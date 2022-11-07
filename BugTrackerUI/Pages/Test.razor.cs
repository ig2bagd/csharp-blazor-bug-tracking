using BugTrackerUI.Data;
using BugTrackerUI.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace BugTrackerUI.Pages
{
   public partial class Test
   {
      string s1 = "Hello";
      string s2 = "<h1>Hello</h1>";

      private DateTime? dte = new DateTime(2022, 1, 1);

      private string account;

      private string wwe = "Bill Goldberg is stronger than Brock Lesnar";

      //[Inject]
      //IHttpClientFactory ClientFactory { get; set; }

      [Inject]
      INorthwindService northwind { get; set; }

      [Parameter]
      public IReadOnlyList<Customer1> customers { get; set; } =
         new List<Customer1>()
         {
         new Customer1("John", 56),
         new Customer1("Sandy", 34),
         new Customer1("Andrew", 45),
         new Customer1("Emily", 47)
         };


      int selectedValue { get; set; }
      protected IReadOnlyList<OrderHist> orderHists = new List<OrderHist>();
      public TelerikNotification NotificationReference { get; set; }

      protected override void OnInitialized()
      {
         selectedValue = 3;
      }

      async Task RetrieveGet()
      {
         //var clientlocal = ClientFactory.CreateClient("AdminApi");
         //var res = await clientlocal.GetStringAsync("api/admin/5");

         orderHists = await northwind.GetCustOrderHist("ANTON");
         StateHasChanged();

         // https://www.telerik.com/forums/why-such-a-bad-implementation
         NotificationReference.Show(new NotificationModel()
         {
            Text = "Auto Closable Notification",
            ThemeColor = ThemeConstants.Notification.ThemeColor.Primary,
            Icon = "check-outline",
            CloseAfter = 5000
         });
      }
   }

}
