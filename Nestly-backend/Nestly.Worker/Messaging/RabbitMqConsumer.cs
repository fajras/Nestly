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

        private IConnection _connection;
        private IModel _channel;

        private readonly string _queueName;
        private readonly string _deadLetterQueue;

        public RabbitMqConsumer(
            IServiceScopeFactory scopeFactory,
            IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _config = config;

            _queueName = _config["RabbitMQ:Queue"];
            _deadLetterQueue = $"{_queueName}.deadletter";

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
                        _channel.BasicNack(
                            ea.DeliveryTag,
                            false,
                            false);
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
                Console.WriteLine(
                    $"RabbitMQ fatal error: {ex}");
            }
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