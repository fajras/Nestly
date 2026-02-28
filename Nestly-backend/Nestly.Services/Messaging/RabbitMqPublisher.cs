using Microsoft.Extensions.Configuration;
using Nestly.Model.DTOObjects;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nestly.Services.Messaging
{
    public class RabbitMqPublisher
    {
        private readonly IConfiguration _config;

        public RabbitMqPublisher(IConfiguration config)
        {
            _config = config;
        }

        public void Publish(NotificationEvent notificationEvent)
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

            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(notificationEvent));

            channel.BasicPublish(
                exchange: "",
                routingKey: _config["RabbitMQ:Queue"],
                basicProperties: null,
                body: body);
        }
    }
}
