using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EventBus.Messages.EventBus
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly string _uri;
        private string _queueName = null!;

        //public string QueueName { get; set; } = null!;

        public RabbitMqEventBus(IConfiguration config)
        {
            _uri = config["EventBus:Uri"]!;
            _queueName = config["EventBus:DefaultQueue"]!;
        }

        public void SetQueueName(string queueName)
        { 
            _queueName = queueName;
        }

        private IModel GetChannel()
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_uri) };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            return channel;
        }

        public void Publish<T>(T message)
        {
            if (string.IsNullOrEmpty(_queueName)) throw new ArgumentNullException("queueName cannot be empty!");

            using var channel = GetChannel();

            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }

        public void Consume(Action<string> messageHandler)
        {
            if (string.IsNullOrEmpty(_queueName)) throw new ArgumentNullException("queueName cannot be empty!");

            using var channel = GetChannel();

            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                messageHandler(message);
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Waiting for messages. Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
