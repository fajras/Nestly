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

        public RabbitMqConsumer(IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = _config["RabbitMQ:Host"],
                        UserName = _config["RabbitMQ:User"],
                        Password = _config["RabbitMQ:Password"]
                    };

                    using var connection = factory.CreateConnection();
                    using var channel = connection.CreateModel();

                    channel.QueueDeclare(
                        queue: _config["RabbitMQ:Queue"],
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += async (model, ea) =>
                    {
                        try
                        {
                            var body = ea.Body.ToArray();
                            var json = Encoding.UTF8.GetString(body);

                            var notificationEvent =
                                JsonSerializer.Deserialize<NotificationEvent>(json);

                            if (notificationEvent == null)
                            {
                                return;
                            }

                            using var scope = _scopeFactory.CreateScope();
                            var db = scope.ServiceProvider
                                          .GetRequiredService<NestlyDbContext>();

                            var notification = new Notification
                            {
                                UserId = notificationEvent.UserId,
                                Title = notificationEvent.Title,
                                Message = notificationEvent.Message
                            };

                            db.Notifications.Add(notification);
                            await db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("RabbitMQ processing ERROR: " + ex.Message);
                        }
                    };

                    channel.BasicConsume(
                        queue: _config["RabbitMQ:Queue"],
                        autoAck: true,
                        consumer: consumer);

                    Console.WriteLine("Worker connected to RabbitMQ and listening...");

                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Rabbit connection failed: {ex.Message}");
                    Console.WriteLine("Retrying in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
}
