﻿using Aspose.Cells;
using BugTrackerUI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BugTrackerUI.Controllers
{
   [Route("api/[controller]/[action]")]
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
      [HttpGet("{customerID}")]
      // https://localhost:44309/api/Admin/CustOrderHist/BLONP
      public async Task<IEnumerable<OrderHist>> CustOrderHist(string customerID)
      {
         SqlParameter param = new SqlParameter("@CustomerID", SqlDbType.NChar, 5);
         param.Value = customerID;

         string sqlQuery = "Exec [dbo].[CustOrderHist] @CustomerID";

         //https://stackoverflow.com/questions/37367282/entity-framework-core-stored-procedure
         return await appDbContext.SqlQueryAsync<OrderHist>(sqlQuery, param);
      }

      [HttpGet]
      [Produces("application/json")]
      // [HttpGet("{id}", Name = "CompanyById")]
      // https://www.telerik.com/blogs/how-to-pass-multiple-parameters-get-method-aspnet-core-mvc
      // https://www.thecodebuzz.com/how-to-pass-multiple-inputs-to-http-get-asp-net-core-guidelines/
      // https://learn.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2#route-constraints
      //public async Task<IEnumerable<SalesByYear>> SalesByYear([FromQuery]DateTime BegDate, [FromQuery]DateTime EndDate)
      // https://localhost:44309/api/Admin/SalesByYear?BegDate=01/01/1998&EndDate=01/10/1998
      public async Task<IEnumerable<SalesByYear>> SalesByYear(DateTime BegDate, DateTime EndDate)
      {
         var prmBegDate = new SqlParameter("@Beginning_Date", SqlDbType.DateTime);
         prmBegDate.Value = BegDate;
         var prmEndDate = new SqlParameter("@Ending_Date", SqlDbType.DateTime);
         prmEndDate.Value = EndDate;

         //object[] array = new[] { prmBegDate, prmEndDate };

         string sqlQuery = "EXEC [dbo].[Sales by Year] @Beginning_Date, @Ending_Date";

         //https://stackoverflow.com/questions/37367282/entity-framework-core-stored-procedure
         //return await appDbContext.SqlQueryAsync<SalesByYear>(sqlQuery, array);
         return await appDbContext.SqlQueryAsync<SalesByYear>(sqlQuery, prmBegDate, prmEndDate);
      }

      [HttpGet]
      //[HttpGet("~/getexcelreport")]
      public IActionResult ExcelReport()
      {
         // https://docs.aspose.com/cells/net/different-ways-to-save-files/#saving-file-to-a-stream
         // https://docs.aspose.com/cells/net/saving-file-to-response-object/
         // https://stackoverflow.com/questions/58527572/is-there-a-way-to-get-a-file-stream-to-download-to-the-browser-in-blazor
         Workbook wb = new Workbook();
         Worksheet ws = wb.Worksheets[0];
         ws.Cells["A1"].PutValue("Hello World!");
         ws.Cells["A2"].PutValue("This is only a test");

         //wb.Save(@"C:\Temp\TEST.xlsx", SaveFormat.Xlsx);
         using (MemoryStream ms = new MemoryStream())
         {
            //wb.Save(ms, new XlsSaveOptions(SaveFormat.Xlsx));
            wb.Save(ms, SaveFormat.Xlsx);
            ms.Position = 0;

            byte[] data = ms.ToArray();
            string fName = "LW300_" + String.Format("{0:MM_dd_yyyy_hh_mm_ss_tt}", DateTime.Now) + ".xlsx";

            if (data != null)
            {
               //return File(data, "application/vnd.ms-excel", fName);                                            // .xls
               return File(data, "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet", fName);    // .xlsx
            }
            else
               //return StatusCode(StatusCodes.Status204NoContent);
               return new EmptyResult();
         }
      }

   }
}
