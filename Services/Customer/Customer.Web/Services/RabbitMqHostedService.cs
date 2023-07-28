using Customer.Utils.Utils;
using EventBus.Contracts;

namespace Customer.Web.Services
{
    public class RabbitMqHostedService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;
        private readonly IEventBus _eventBus;

        public RabbitMqHostedService(IConfiguration config, IWebHostEnvironment environment, IEventBus eventBus)
        {
            _config = config;
            _environment = environment;
            _eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _eventBus.Consume(message =>
            {
                try
                {
                    var path = Path.Combine(_environment.WebRootPath, _config["Data:FilePath"]!);
                    CsvFileUtils.Append(message, path);

                    // TODO: send Ack
                }
                catch (Exception exc)
                {
                    // TODO: send Nack
                }
            });

            await Task.CompletedTask;
        }
    }
}
