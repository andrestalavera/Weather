using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data;
using Weather.Models;

namespace Weather.Api.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class NewDataMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<NewDataMiddleware> _logger;
        private readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public NewDataMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<NewDataMiddleware>();
        }

        public Task Invoke(HttpContext httpContext, WeatherForecastDbContext context)
        {
            _logger.LogInformation($"Invoke {nameof(NewDataMiddleware)}");
            if (!context.WeatherForecasts.Any())
            {
                _logger.LogInformation($"Table `{nameof(WeatherForecast)}` does not contain any items");
                var rng = new Random();
                _logger.LogInformation($"Attempt to generate 50 new items");
                var items = Enumerable.Range(0, 22).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                });
                _logger.LogInformation($"Attempt to ensure that database is created");
                context.Database.EnsureCreated();
                _logger.LogInformation($"Attempt to add 50 new items");
                context.WeatherForecasts.AddRangeAsync(items);
                context.SaveChangesAsync();
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class NewDataMiddlewareExtensions
    {
        public static IApplicationBuilder UseNewData(this IApplicationBuilder builder) => builder.UseMiddleware<NewDataMiddleware>();
    }
}
