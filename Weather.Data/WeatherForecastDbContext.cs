using Microsoft.EntityFrameworkCore;
using Weather.Models;

namespace Weather.Data
{
    public class WeatherForecastDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) 
            : base(options)
        {
        }
    }
}