using Customer.Utils.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Customer.Web.EventBus
{
    public class RabbitMqHostedService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly AsyncEventingBasicConsumer _consumer;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;
        private string _queue;        

        public RabbitMqHostedService(IConfiguration config, IWebHostEnvironment environment)
        {
            _config = config;
            _environment = environment;

            var factory = new ConnectionFactory { Uri = new Uri(config["EventBus:Uri"]!), DispatchConsumersAsync = true };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _queue = config["EventBus:CustomerDataQueue"]!;

            _channel.QueueDeclare(queue: _queue, exclusive: false, autoDelete: false);

            _consumer = new(_channel);            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _consumer.Received += async (model, eventArgs) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                    var path = Path.Combine(_environment.WebRootPath, _config["Data:FilePath"]!);

                    CsvFileUtils.Append(message, path);

                    // Acknowledge that the message has been successfully processed and can be safely removed from the queue.
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception exc)
                {
                    // Reject the delivered message and ask RabbitMQ to requeue them or discard them based on the requeue parameter.
                    _channel.BasicNack(eventArgs.DeliveryTag, false, false);
                }
            };

            // Register the consumer for the queue.
            // If autoAck is set to true, message is considered as acknowledged and removed from the queue
            // as soon as it is delivered to the consumer.
            // The consumer does not need to explicitly call BasicAck to acknowledge the messages.
            _channel.BasicConsume(queue: _queue, autoAck: false, consumer: _consumer);

            await Task.CompletedTask;
        }
    }
}
