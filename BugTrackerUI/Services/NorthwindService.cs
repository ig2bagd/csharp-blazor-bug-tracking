using BugTrackerUI.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using System.Text.Json;

namespace BugTrackerUI.Services
{
   public class NorthwindService : INorthwindService
   {
      private readonly HttpClient client;
      private readonly JsonSerializerOptions _options;
      //private readonly NavigationManager navMgr;
      private IReadOnlyList<OrderHist> Orders;
      private IEnumerable<SalesByYear> Sales;

      // Typed client accepts an HttpClient parameter in its constructor
      // https://stackoverflow.com/questions/63828177/how-to-configure-httpclient-base-address-in-blazor-server-using-ihttpclientfacto
      //public NorthwindService(HttpClient client, NavigationManager navMgr)
      public NorthwindService(HttpClient client)
      {
         this.client = client;
         //this.navMgr = navMgr;
         //client.BaseAddress = new Uri(navMgr.BaseUri);

         _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
      }

      public async Task<IReadOnlyList<OrderHist>> GetCustOrderHist(string customerID)
      {
         //var client = factory.CreateClient("AdminApi");
         //return client.GetFromJsonAsync<IEnumerable<OrderHist>>($"api/admin/CustOrderHist/{customerID}");

         //var request = new HttpRequestMessage(HttpMethod.Get, $"api/Admin/CustOrderHist/{customerID}");
         //var res = await client.SendAsync(request);
         HttpResponseMessage res = await client.GetAsync($"api/Admin/CustOrderHist/{customerID}");
         //res.EnsureSuccessStatusCode();

         if (res.IsSuccessStatusCode)
         {
            // Only deserialize when we did not have an API failure
            //OrderHists = await res.Content.ReadFromJsonAsync<List<OrderHist>>().ConfigureAwait(false);
            Orders = await res.Content.ReadFromJsonAsync<IReadOnlyList<OrderHist>>();
            return Orders;
         }
         else
         {
            // Otherwise treat the response as an error message
            string errMsg = await res.Content.ReadAsStringAsync();
            Console.WriteLine(errMsg);
            throw new Exception(errMsg);
         }
      }

      /*
      public async Task<IEnumerable<SalesByYear>> GetSalesByYear(DateTime BegDate, DateTime EndDate)
      {
         // https://www.ezzylearning.net/tutorial/making-http-requests-in-blazor-server-apps
         //var uri = string.Format("api/Admin/SalesByYear?BegDate={0}&EndDate={1}", BegDate, EndDate);
         var res = await client.GetAsync($"api/Admin/SalesByYear?BegDate={BegDate}&EndDate={EndDate}");
         //res.EnsureSuccessStatusCode();

         if (res.IsSuccessStatusCode)
         {
            Sales = await res.Content.ReadFromJsonAsync<IEnumerable<SalesByYear>>();
            return Sales;
         }
         else
         {
            string errMsg = await res.Content.ReadAsStringAsync();
            Console.WriteLine(errMsg);
            throw new Exception(errMsg);
         }
      }
      */

      // Method #2: Using stream
      public async Task<IEnumerable<SalesByYear>> GetSalesByYear(DateTime BegDate, DateTime EndDate)
      {
         // https://code-maze.com/using-streams-with-httpclient-to-improve-performance-and-memory-usage/
         using (var res = await client.GetAsync($"api/Admin/SalesByYear?BegDate={BegDate}&EndDate={EndDate}", HttpCompletionOption.ResponseHeadersRead))
         {
            res.EnsureSuccessStatusCode();

            var stream = await res.Content.ReadAsStreamAsync();
            Sales = await JsonSerializer.DeserializeAsync<List<SalesByYear>>(stream, _options);
            return Sales;
         }
      }

   }

}
