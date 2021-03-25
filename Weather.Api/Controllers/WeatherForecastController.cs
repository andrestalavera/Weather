using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Weather.Common.Pagination;
using Weather.Data;
using Weather.Models;

namespace Weather.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastDbContext _context;

        public WeatherForecastController(WeatherForecastDbContext context, ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<PaginatedList<WeatherForecast>> Get(int page = PageSettings.DEFAULT_PAGE_NUMBER, int size = PageSettings.DEFAULT_PAGE_SIZE)
        {
            var query = _context.WeatherForecasts.AsNoTracking();
            var count = await query.CountAsync();
            _logger.LogInformation($"Total: {count}");

            if (page > 1)
            {
                _logger.LogInformation($"Page Number: {page}");
                query = query.Skip((page - 1) * size);
            }
            _logger.LogInformation($"Page size: {size}");
            query = query.Take(size);

            _logger.LogInformation($"Attempt to execute the query");
            var items = await query.ToListAsync();

            _logger.LogInformation($"Attempt to build paginated list");
            return new PaginatedList<WeatherForecast>
            {
                Page = page,
                Size = size,
                Total = count,
                Items = items,
                Pages = (int)Math.Ceiling((decimal)count / size)
            };
        }
    }
}
