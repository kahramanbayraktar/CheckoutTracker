using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EventBus.Messages.Events
{
    public class CheckOutDataPublisher : IMessagePublisher
    {
        private const string QueueName = "checkout-data";

        public void Publish<T>(T message)
        {
            //var factory = new ConnectionFactory { HostName = "rabbitmq" };
            var factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@rabbitmq:5672") };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            #region
            // exclusive is set to false, otherwise it causes the below error:
            // RabbitMQ.Client.Exceptions.OperationInterruptedException: The AMQP operation was interrupted: AMQP close-reason, initiated by Peer, code=405, text=’RESOURCE_LOCKED – cannot obtain exclusive access to locked queue in ‘orders’ vhost ‘/’.
            // https://github.com/pardahlman/RawRabbit/issues/192
            #endregion
            channel.QueueDeclare(QueueName, exclusive: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: QueueName, body: body);
        }
    }
}
