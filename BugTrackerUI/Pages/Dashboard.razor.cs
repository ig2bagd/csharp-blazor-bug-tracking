using BugTrackerUI.Data;
using BugTrackerUI.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BugTrackerUI.Pages
{
   public partial class Dashboard
   {
      [Inject]
      private IOrderService OrderService { get; set; }

      [Inject]
      private IProductService ProductService { get; set; }

      private List<Order> Orders { get; set; }
      private List<Product> Products { get; set; }

      protected override void OnInitialized()
      {
         Orders = OrderService.GetLatestOrders();
         Products = ProductService.GetTopSellingProducts();
      }
   }
}
