using System;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerUI.Data
{
   public class WeatherForecastService
   {
      private static readonly string[] Summaries = new[]
      {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

      public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
      {
         Random rnd = new Random();

         return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
         {
            Date = startDate.AddDays(index),
            TemperatureC = rnd.Next(-20, 55),
            Summary = Summaries[rnd.Next(Summaries.Length)]
         }).ToArray());
      }
   }
}
