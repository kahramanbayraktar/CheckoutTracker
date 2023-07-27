using CheckoutDataParser.EventBus;
using DataParser.Parser;
using EventBus.Messages.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace DataParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(sp => config);
            services.AddSingleton<IMessageProducer, RabbitMqMessageProducer>();

            // TODO: Looks redundant
            var provider = services.BuildServiceProvider();
            var publisher = (RabbitMqMessageProducer)provider.GetService<IMessageProducer>()!;

            // LISTEN
            RabbitMqMessageConsumer rabbitMqConsumer = new(config);

            //var factory = new ConnectionFactory { Uri = new Uri(config["EventBus:Uri"]!) };
            //var connection = factory.CreateConnection();
            //var channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(rabbitMqConsumer.Channel);
            //var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, eventArgs) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    var checkoutData = JsonSerializer.Deserialize<CheckoutData>(message);
                    var customerData = MessageParser.Parse(checkoutData!);

                    // PUBLISH
                    publisher.Publish(customerData);

                    // Acknowledge that the message has been successfully processed and can be safely removed from the queue.
                    rabbitMqConsumer.Channel.BasicAck(eventArgs.DeliveryTag, false);

                    Console.WriteLine(message);
                }
                catch (Exception exc)
                {
                    // Reject the delivered message and ask RabbitMQ to requeue them or discard them based on the requeue parameter.
                    rabbitMqConsumer.Channel.BasicNack(eventArgs.DeliveryTag, false, false);
                }                
            };

            // Register the consumer for the queue.
            rabbitMqConsumer.Channel.BasicConsume(queue: rabbitMqConsumer.Channel.CurrentQueue, autoAck: false, consumer: consumer);

            // Keep the application running
            Console.WriteLine("Consumer is running. Press any key to exit.");
            Console.ReadLine();
        }
    }
}