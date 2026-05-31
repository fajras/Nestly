using Microsoft.Extensions.Configuration;
using Nestly.Model.DTOObjects;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Nestly.Services.Messaging
{
    public class RabbitMqPublisher : IDisposable
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMqPublisher(IConfiguration config)
        {
            _config = config;

            _queueName = _config["RabbitMQ:Queue"];

            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:Host"],
                UserName = _config["RabbitMQ:User"],
                Password = _config["RabbitMQ:Password"],
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", $"{_queueName}.deadletter" }
            };

            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: args);

            _channel.QueueDeclare(
                queue: $"{_queueName}.deadletter",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void Publish(NotificationEvent notificationEvent)
        {
            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(notificationEvent));

            var properties = _channel.CreateBasicProperties();

            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: properties,
                body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();

            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}