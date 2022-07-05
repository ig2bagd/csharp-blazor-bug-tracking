using BugTrackerUI.Data;
using System.Collections.Generic;

namespace BugTrackerUI.Services
{
   public interface IOrderService
   {
      List<Order> GetLatestOrders();
   }
}