using Fluxor;
using Microsoft.AspNetCore.Components;
using BugTrackerUI.Store.CounterUseCase;
using BugTrackerUI.Store;
using System;

namespace BugTrackerUI.Pages
{
   partial class Counter
   {
      [Inject]
      private IState<CounterState> CounterState { get; set; }

      [Inject]
      public IDispatcher Dispatcher { get; set; }

      [Parameter]
      public Action<int> OnMultipleOfTwoAction { get; set; }

      [Parameter]
      public EventCallback<int> OnMultipleOfThree { get; set; }

      private void IncrementCount()
      {
         var action = new IncrementCounterAction();
         Dispatcher.Dispatch(action);

         if (CounterState.Value.ClickCount % 2 == 0)
            OnMultipleOfTwoAction?.Invoke(CounterState.Value.ClickCount);

         if (CounterState.Value.ClickCount % 3 == 0)
            OnMultipleOfThree.InvokeAsync(CounterState.Value.ClickCount);
      }
   }
}
