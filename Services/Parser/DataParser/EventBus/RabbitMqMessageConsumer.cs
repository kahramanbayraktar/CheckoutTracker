using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace CheckoutDataParser.EventBus
{
    public class RabbitMqMessageConsumer : IMessageConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        
        public RabbitMqMessageConsumer(IConfiguration config)
        {
            var factory = new ConnectionFactory { Uri = new Uri(config["EventBus:Uri"]!) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(config["EventBus:CheckoutDataQueue"], exclusive: false);
        }

        public IModel Channel => _channel;

        // TODO
        public void Consume()
        {        
        }
    }
}
