using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
namespace Nestly.Services.Messaging
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;
        private readonly IHubContext<NotificationHub> _hub;

        public RabbitMqConsumer(IServiceScopeFactory scopeFactory, IConfiguration config, IHubContext<NotificationHub> hub)
        {
            _scopeFactory = scopeFactory;
            _config = config;
            _hub = hub;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:Host"],
                UserName = _config["RabbitMQ:User"],
                Password = _config["RabbitMQ:Password"]
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

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
                    var db = scope.ServiceProvider.GetRequiredService<NestlyDbContext>();

                    var notification = new Notification
                    {
                        UserId = notificationEvent.UserId,
                        Title = notificationEvent.Title,
                        Message = notificationEvent.Message
                    };

                    db.Notifications.Add(notification);
                    await db.SaveChangesAsync();

                    await _hub.Clients
                        .Group($"user-{notificationEvent.UserId}")
                        .SendAsync("ReceiveNotification", new
                        {
                            title = notificationEvent.Title,
                            message = notificationEvent.Message
                        });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("RabbitMQ ERROR: " + ex.Message);
                }
            };

            channel.BasicConsume(
                queue: _config["RabbitMQ:Queue"],
                autoAck: true,
                consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
