using BugTrackerUI.Data;
using System.Collections.Generic;

namespace BugTrackerUI.Store
{
   public class FetchDataResultAction
   {
      public IEnumerable<WeatherForecast> Forecasts { get; }

      public FetchDataResultAction(IEnumerable<WeatherForecast> forecasts)
      {
         Forecasts = forecasts;
      }
   }
}
