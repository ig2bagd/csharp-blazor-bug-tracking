using BugTrackerUI.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTrackerUI.Services
{
   public interface INorthwindService
   {
      Task<IReadOnlyList<OrderHist>> GetCustOrderHist(string customerID);
      Task<IEnumerable<SalesByYear>> GetSalesByYear(DateTime BegDate, DateTime EndDate);
   }
}