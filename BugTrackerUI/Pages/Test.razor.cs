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

using Aspose.Cells;
using System.IO;

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


      int selectedValue { get; set; }
      protected IReadOnlyList<OrderHist> orderHists = new List<OrderHist>();
      protected IEnumerable<SalesByYear> salesByYear = new List<SalesByYear>();
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
         salesByYear = await northwind.GetSalesByYear(DateTime.Parse("01/01/1998"), DateTime.Parse("01/10/1998"));
         //StateHasChanged();

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

      async Task ExportExcel() => await Task.Run(() => Navigation.NavigateTo($"/getexcelreport", true));
      async Task ExportExcel2()
      {
         /*
         // https://docs.aspose.com/cells/net/different-ways-to-save-files/#saving-file-to-a-stream
         // https://docs.aspose.com/cells/net/saving-file-to-response-object/
         // https://stackoverflow.com/questions/58527572/is-there-a-way-to-get-a-file-stream-to-download-to-the-browser-in-blazor
         Workbook wb = new Workbook();
         Worksheet ws = wb.Worksheets[0];
         ws.Cells["A1"].PutValue("Hello World!");
         ws.Cells["B1"].PutValue("This is only a test");

         //wb.Save(@"C:\Temp\TEST.xlsx", SaveFormat.Xlsx);

         using (MemoryStream ms = new MemoryStream())
         {
            //wb.Save(ms, new XlsSaveOptions(SaveFormat.Xlsx));
            wb.Save(ms, SaveFormat.Xlsx);
            ms.Position = 0;

            byte[] sheetData = ms.ToArray();
         }
         */

         Navigation.NavigateTo($"/getexcelreport", true);

         //await Task.Delay(1000);
         await Task.CompletedTask;
      }
      

   }

}
