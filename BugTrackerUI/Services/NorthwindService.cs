using BugTrackerUI.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BugTrackerUI.Services
{
   public class NorthwindService : INorthwindService
   {
      private readonly HttpClient client;
      private IReadOnlyList<OrderHist> Orders;

      // Typed client accepts an HttpClient parameter in its constructor
      public NorthwindService(HttpClient client)
      {
         this.client = client;
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

   }

}
