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

            _channel.QueueDeclare(queue: _queue, exclusive: false);

            _consumer = new(_channel);            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            //var consumer = new AsyncEventingBasicConsumer(_channel);

            _consumer.Received += async (model, eventArgs) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                    var path = Path.Combine(_environment.WebRootPath, _config["Data:FilePath"]!);

                    CsvFileUtils.Append(message, path);

                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception exc)
                {
                    _channel.BasicNack(eventArgs.DeliveryTag, false, false);
                }
            };

            _channel.BasicConsume(queue: _queue, autoAck: false, consumer: _consumer);

            await Task.CompletedTask;
        }
    }
}
