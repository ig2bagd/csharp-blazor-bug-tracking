using BugTrackerUI.Data;
using System;
using System.Collections.Generic;

namespace BugTrackerUI.Services
{
   public class OrderService : IOrderService
   {
      public List<Order> GetLatestOrders()
      {
         return new List<Order>()
         {
            new Order()
            {
                Id = 1,
                OrderNo = "12345",
                OrderDate = DateTime.Today.AddDays(-2),
                Status = "Pending",
                OrderTotal = 399.99m
            },
            new Order()
            {
                Id = 2,
                OrderNo = "67890",
                OrderDate = DateTime.Today.AddDays(-5),
                Status = "Completed",
                OrderTotal = 199.99m
            },
            new Order()
            {
                Id = 3,
                OrderNo = "13579",
                OrderDate = DateTime.Today.AddDays(-7),
                Status = "Completed",
                OrderTotal = 249.99m
            }
         };
      }
   }
}