using BugTrackerUI.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BugTrackerUI.Services
{
   public class UrbanAreaService: IUrbanAreaService
   {
      private HttpClient _http;

      public UrbanAreaService(HttpClient http)
      {
         _http = http;
      }

      public Task<IEnumerable<UrbanAreaDto>> GetUrbanAreas()
      {
         return _http.GetFromJsonAsync<IEnumerable<UrbanAreaDto>>("https://demos.telerik.com/blazor-ui-service/api/UrbanArea/GetUrbanAreas");
      }
   }

   public interface IUrbanAreaService 
   {
      Task<IEnumerable<UrbanAreaDto>> GetUrbanAreas();
   }
}
