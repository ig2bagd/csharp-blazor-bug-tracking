using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BugTrackerUI.Services
{
   public class JsConsole
   {
      private readonly IJSRuntime JS;
      public JsConsole(IJSRuntime jsRuntime)
      {
         JS = jsRuntime;
      }

      public async Task LogAsync(object obj)
      {
         await JS.InvokeVoidAsync("console.log", obj);
      }
   }
}
