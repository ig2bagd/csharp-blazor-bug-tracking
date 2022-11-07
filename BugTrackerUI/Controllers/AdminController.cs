using BugTrackerUI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BugTrackerUI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class AdminController : ControllerBase
   {
      private readonly AppDbContext appDbContext;

      public AdminController(AppDbContext appDbContext)
      {
         this.appDbContext = appDbContext;
      }

      // GET: api/<AdminController>
      [HttpGet]
      public IEnumerable<string> Get()
      {
         return new string[] { "value1", "value2" };
      }

      // GET: api/<AdminController>/5
      [HttpGet("{id}")]
      public string Get(int id)
      {
         return id.ToString();
      }

      //https://makolyte.com/aspnetcore-the-request-matched-multiple-endpoints/
      //https://code-maze.com/aspnetcore-webapi-add-multiple-post-actions/
      //[Route("CustOrderHist")]
      //[HttpGet("{customerID:alpha}")]
      [HttpGet("[action]/{customerID}")]
      public async Task<IEnumerable<OrderHist>> CustOrderHist(string customerID)
      {
         SqlParameter param = new SqlParameter("@CustomerID", SqlDbType.NChar, 5);
         param.Value = customerID;

         string sqlQuery = "Exec [dbo].[CustOrderHist] @CustomerID";

         //https://stackoverflow.com/questions/37367282/entity-framework-core-stored-procedure
         return await appDbContext.SqlQueryAsync<OrderHist>(sqlQuery, param);
      }
   }
}
