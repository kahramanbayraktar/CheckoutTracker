using Checkout.API.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Checkout.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication? app, ILogger logger)
        {
            app!.UseExceptionHandler(err =>
            {
                err.Run(async context =>
                {
                    context.Response.StatusCode = HttpStatusCode.InternalServerError.GetHashCode();
                    context.Response.ContentType = "application/json";

                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature != null)
                    {
                        logger.LogError($"An error occurred: {feature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error"
                        }.ToString());
                    }
                });
            });
        }
    }
}