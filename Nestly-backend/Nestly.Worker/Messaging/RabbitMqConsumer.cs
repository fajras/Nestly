using Microsoft.Extensions.Logging;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Nestly.Worker.Messaging
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<RabbitMqConsumer> _logger;

        private IConnection _connection;
        private IModel _channel;

        private readonly string _queueName;
        private readonly string _deadLetterQueue;
        private readonly string _retryQueue;

        // A failed message is retried a few times (via a short-TTL "retry"
        // queue that dead-letters back into the main queue) before it is
        // finally routed to the real dead-letter queue. This covers
        // transient failures (e.g. a momentary DB blip) that would
        // otherwise dead-letter a message on the very first failure.
        private const int MaxRetryAttempts = 3;
        private const int RetryDelayMs = 15000;
        private const string RetryCountHeader = "x-retry-count";

        public RabbitMqConsumer(
            IServiceScopeFactory scopeFactory,
            IConfiguration config,
            ILogger<RabbitMqConsumer> logger)
        {
            _scopeFactory = scopeFactory;
            _config = config;
            _logger = logger;

            _queueName = _config["RabbitMQ:Queue"];
            _deadLetterQueue = $"{_queueName}.deadletter";
            _retryQueue = $"{_queueName}.retry";

            InitializeRabbitMq();
        }

        private void InitializeRabbitMq()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:Host"],
                UserName = _config["RabbitMQ:User"],
                Password = _config["RabbitMQ:Password"],
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", _deadLetterQueue }
            };

            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: args);

            _channel.QueueDeclare(
                queue: _deadLetterQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Messages sit here for RetryDelayMs, then their TTL expires and
            // RabbitMQ dead-letters them straight back into the main queue
            // for another processing attempt - a delayed-retry queue built
            // from plain TTL + DLX, no plugin required.
            var retryArgs = new Dictionary<string, object>
            {
                { "x-message-ttl", RetryDelayMs },
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", _queueName }
            };

            _channel.QueueDeclare(
                queue: _retryQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: retryArgs);

            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            try
            {

                var consumer =
                    new AsyncEventingBasicConsumer(
                        _channel);

                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var body =
                            ea.Body.ToArray();

                        var json =
                            Encoding.UTF8.GetString(body);

                        var notificationEvent =
                            JsonSerializer.Deserialize<NotificationEvent>(json);

                        if (notificationEvent == null)
                        {
                            // Malformed payload: retrying would never help,
                            // so this goes straight to the dead-letter queue.
                            _channel.BasicNack(
                                ea.DeliveryTag,
                                false,
                                false);

                            return;
                        }

                        using var scope =
                            _scopeFactory.CreateScope();

                        var db =
                            scope.ServiceProvider
                                .GetRequiredService<NestlyDbContext>();

                        var notification =
                            new Notification
                            {
                                UserId =
                                    notificationEvent.UserId,

                                Title =
                                    notificationEvent.Title,

                                Message =
                                    notificationEvent.Message,

                                CreatedAt =
                                    DateTime.UtcNow,

                                IsRead = false
                            };

                        db.Notifications.Add(
                            notification);

                        await db.SaveChangesAsync(
                            stoppingToken);

                        _channel.BasicAck(
                            ea.DeliveryTag,
                            false);
                    }
                    catch (Exception ex)
                    {
                        var retryCount = GetRetryCount(ea.BasicProperties);

                        if (retryCount < MaxRetryAttempts)
                        {
                            _logger.LogWarning(
                                ex,
                                "Failed to process notification message (delivery tag {DeliveryTag}), scheduling retry {RetryCount}/{MaxRetryAttempts}.",
                                ea.DeliveryTag, retryCount + 1, MaxRetryAttempts);

                            RepublishForRetry(ea, retryCount + 1);

                            // The original delivery is acked because a copy
                            // with the incremented retry count was just
                            // published to the retry queue - leaving the
                            // original unacked as well would process it twice.
                            _channel.BasicAck(ea.DeliveryTag, false);
                        }
                        else
                        {
                            _logger.LogError(
                                ex,
                                "Failed to process notification message (delivery tag {DeliveryTag}) after {MaxRetryAttempts} attempts, sending to dead-letter queue.",
                                ea.DeliveryTag, MaxRetryAttempts);

                            _channel.BasicNack(
                                ea.DeliveryTag,
                                false,
                                false);
                        }
                    }
                };

                _channel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);

                await Task.Delay(
                    Timeout.Infinite,
                    stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    ex,
                    "RabbitMQ fatal error.");
            }
        }


        private static int GetRetryCount(IBasicProperties properties)
        {
            if (properties?.Headers != null &&
                properties.Headers.TryGetValue(RetryCountHeader, out var value))
            {
                return value switch
                {
                    int i => i,
                    long l => (int)l,
                    byte[] bytes => int.TryParse(Encoding.UTF8.GetString(bytes), out var parsed) ? parsed : 0,
                    _ => 0
                };
            }

            return 0;
        }

        private void RepublishForRetry(BasicDeliverEventArgs ea, int newRetryCount)
        {
            var retryProperties = _channel.CreateBasicProperties();
            retryProperties.Persistent = true;
            retryProperties.Headers = ea.BasicProperties?.Headers != null
                ? new Dictionary<string, object>(ea.BasicProperties.Headers)
                : new Dictionary<string, object>();
            retryProperties.Headers[RetryCountHeader] = newRetryCount;

            _channel.BasicPublish(
                exchange: "",
                routingKey: _retryQueue,
                basicProperties: retryProperties,
                body: ea.Body.ToArray());
        }

        public override void Dispose()
        {
            try
            {
                _channel?.Close();
                _connection?.Close();

                _channel?.Dispose();
                _connection?.Dispose();
            }
            catch
            {
            }

            base.Dispose();
        }
    }
}