using DataParser.Parser;
using EventBus.Implementations;
using EventBus.Messages.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace DataParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(sp => config);
            //services.AddSingleton<IEventBus, RabbitMqEventBus>(); // TODO: make this transient

            // TODO: Looks redundant
            //var provider = services.BuildServiceProvider();
            //var eventBus = (RabbitMqEventBus)provider.GetService<IEventBus>()!;

            RabbitMqEventBus eventBus = new(config);
            eventBus.SetQueueName(config["EventBus:DefaultQueue"]!); // TODO: a better way?
            eventBus.Consume(x =>
            {
                eventBus.SetQueueName(config["EventBus:CustomerDataQueue"]!); // TODO: a better way?

                var checkoutData = JsonSerializer.Deserialize<CheckoutData>(x);
                var customerData = MessageParser.Parse(checkoutData!);

                eventBus.Publish(customerData);
                Console.WriteLine(customerData);
            });

            Console.WriteLine("test");
        }
    }
}