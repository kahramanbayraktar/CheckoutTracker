using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CheckoutDataParser.EventBus
{
    public class RabbitMqMessageProducer : IMessageProducer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private string _queue;

        public RabbitMqMessageProducer(IConfiguration config)
        {
            var factory = new ConnectionFactory { Uri = new Uri(config["EventBus:Uri"]!) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _queue = config["EventBus:CustomerDataQueue"]!;

            #region
            // exclusive is set to false, otherwise it causes the below error:
            // RabbitMQ.Client.Exceptions.OperationInterruptedException: The AMQP operation was interrupted: AMQP close-reason, initiated by Peer, code=405, text=’RESOURCE_LOCKED – cannot obtain exclusive access to locked queue in ‘orders’ vhost ‘/’.
            // https://github.com/pardahlman/RawRabbit/issues/192
            #endregion
            _channel.QueueDeclare(_queue, exclusive: false);
        }

        public void Publish<T>(T message)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.BasicPublish(exchange: "", routingKey: _queue, body: body);
        }
    }
}
