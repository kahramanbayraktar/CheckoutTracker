using EventBus.Implementations;
using EventBus.Contracts;
using MediatR;
using System.Reflection;

namespace Checkout.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            builder.Services.AddControllers();

            builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}