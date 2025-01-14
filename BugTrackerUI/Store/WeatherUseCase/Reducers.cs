﻿using Fluxor;

namespace BugTrackerUI.Store.WeatherUseCase
{
   public static class Reducers
   {
      [ReducerMethod]
      public static WeatherState ReduceFetchDataAction(WeatherState state, FetchDataAction action) =>
        new WeatherState(
          isLoading: true,
          forecasts: null);

      [ReducerMethod]
      public static WeatherState ReduceFetchDataResultAction(WeatherState state, FetchDataResultAction action) =>
        new WeatherState(
          isLoading: false,
          forecasts: action.Forecasts);
   }
}
