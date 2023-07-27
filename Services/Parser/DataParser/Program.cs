using CheckoutDataParser.EventBus;
using DataParser.Parser;
using EventBus.Messages.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DataParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var checkoutDataQueue = config["RabbitMq:CheckoutDataQueue"];

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(sp => config);
            services.AddSingleton<IMessageProducer, RabbitMqMessageProducer>();

            // TODO: Look redundant
            var provider = services.BuildServiceProvider();
            var publisher = (RabbitMqMessageProducer)provider.GetService<IMessageProducer>()!;

            // LISTEN
            RabbitMqMessageConsumer rabbitMqConsumer = new(config);

            var consumer = new EventingBasicConsumer(rabbitMqConsumer.Channel);

            consumer.Received += (model, eventArgs) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    var checkoutData = JsonSerializer.Deserialize<CheckoutData>(message);
                    var customerData = MessageParser.Parse(checkoutData!);

                    // PUBLISH
                    publisher.Publish(customerData);

                    rabbitMqConsumer.Channel.BasicAck(eventArgs.DeliveryTag, false);

                    Console.WriteLine(message);
                }
                catch (Exception exc)
                {
                    rabbitMqConsumer.Channel.BasicNack(eventArgs.DeliveryTag, false, false);
                }                
            };

            // CONSUME
            rabbitMqConsumer.Channel.BasicConsume(queue: rabbitMqConsumer.Channel.CurrentQueue, autoAck: true, consumer: consumer);

            // Keep the application running
            Console.WriteLine("Consumer is running. Press any key to exit.");
            Console.ReadLine();
        }
    }
}