using CheckoutDataParser.Entities;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CheckoutDataParser.EventBus
{
    //public class CustomerDataPublisher
    //{
    //    private readonly RabbitMqPublisher _rabbitMqService;
    //    private readonly IConfiguration _config;

    //    public CustomerDataPublisher(RabbitMqPublisher rabbitMqService, IConfiguration config)
    //    {
    //        _rabbitMqService = rabbitMqService;
    //        _config = config;
    //    }

    //    public void Send(CustomerData customerInfo)
    //    {
    //        var channel = _rabbitMqService.CreateChannel("amqp://guest:guest@rabbitmq:5672");

    //        var json = JsonSerializer.Serialize(customerInfo);
    //        var body = Encoding.UTF8.GetBytes(json);

    //        channel.BasicPublish(exchange: "", routingKey: _config["EventBus:DefaultQueue"], body: body);
    //    }
    //}
}
