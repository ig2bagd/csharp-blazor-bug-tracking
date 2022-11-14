using BugTrackerUI.Data;
using BugTrackerUI.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;


namespace BugTrackerUI.Pages
{
   public partial class Test : IDisposable
   {
      string s1 = "Hello";
      string s2 = "<h1>Hello</h1>";

      private DateTime? dte = new DateTime(2022, 1, 1);

      private string account;

      private string wwe = "Bill Goldberg is stronger than Brock Lesnar";

      //[Inject]
      //IHttpClientFactory ClientFactory { get; set; }

      //[Inject] HttpClient client { get; set; } 

      [Inject] INorthwindService northwind { get; set; }

      [Inject] NavigationManager Navigation { get; set; }

      [Parameter]
      public IReadOnlyList<Customer1> customers { get; set; } =
         new List<Customer1>()
         {
            new Customer1("John", 56),
            new Customer1("Sandy", 34),
            new Customer1("Andrew", 45),
            new Customer1("Emily", 47)
         };

      private TelerikDropDownList<OrderHist, int> DropDownRef { get; set; }
      int SelectedValue { get; set; }
      protected IReadOnlyList<OrderHist> orderHists = new List<OrderHist>();
      protected IEnumerable<SalesByYear> salesByYear = new List<SalesByYear>();
      public TelerikNotification NotificationReference { get; set; }

      private CancellationTokenSource source = new CancellationTokenSource(5000);

      protected override void OnInitialized()
      {
         SelectedValue = 3;
      }

      async Task RetrieveGet()
      {
         //var clientlocal = ClientFactory.CreateClient("AdminApi");
         //var res = await clientlocal.GetStringAsync("api/admin/5");

         orderHists = await northwind.GetCustOrderHist("ANTON");
         salesByYear = await northwind.GetSalesByYear(DateTime.Parse("01/01/1998"), DateTime.Parse("01/10/1998"));
         //StateHasChanged();
         DropDownRef.Rebind();

         // https://www.telerik.com/forums/why-such-a-bad-implementation
         NotificationReference.Show(new NotificationModel()
         {
            Text = "Auto Closable Notification",
            //ThemeColor = ThemeConstants.Notification.ThemeColor.Primary,
            ThemeColor = ThemeConstants.Notification.ThemeColor.Info,
            Icon = "info-circle",
            CloseAfter = 5000
         });

      }

      //async Task ExportExcel() => await Task.Run(() => Navigation.NavigateTo($"/getexcelreport", true));
      async Task ExportExcel() => await Task.Run(() => Navigation.NavigateTo($"/api/admin/excelreport", true));

      async Task ExportExcel2()
      {
         //Navigation.NavigateTo($"/getexcelreport", true);
         Navigation.NavigateTo($"/api/admin/excelreport", true);

         //await Task.Delay(1000);
         await Task.CompletedTask;
      }

      public void Dispose()
      {
         source.Cancel();
         source.Dispose();
      }
   }

}
