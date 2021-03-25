using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Weather.Common.Pagination;
using Weather.Models;

namespace Weather.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public PaginatedList<WeatherForecast> Get(int page = PageSettings.DEFAULT_PAGE_NUMBER, int size = PageSettings.DEFAULT_PAGE_SIZE)
        {
            var rng = new Random();
            var items = Enumerable.Range(10, 50).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
            return new PaginatedList<WeatherForecast>
            {
                Page = page,
                Size = size,
                Total = items.Count(),
                Items = items
            };
        }
    }
}
