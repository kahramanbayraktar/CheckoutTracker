using Checkout.API.Extensions;
using EventBus.Contracts;
using EventBus.Implementations;
using MediatR;
using System.Reflection;

namespace Checkout.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container.

            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            builder.Services.AddControllers();

            builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

            var app = builder.Build();

            var logger = app.Services.GetService<ILoggerFactory>()!.CreateLogger<Program>();

            // Configure the HTTP request pipeline.

            app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}