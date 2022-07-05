using BugTrackerUI.Data;
using System.Collections.Generic;

namespace BugTrackerUI.Services
{
   public class ProductService : IProductService
   {
      public List<Product> GetTopSellingProducts()
      {
         return new List<Product>()
         {
            new Product()
            {
                Id = 1,
                Title = "Wireless Mouse",
                Price = 29.99m,
                Quantity = 3
            },
            new Product()
            {
                Id = 2,
                Title = "HP Headphone",
                Price = 79.99m,
                Quantity = 4
            },
            new Product()
            {
                Id = 3,
                Title = "Sony Keyboard",
                Price = 119.99m,
                Quantity = 5
            }
         };
      }
   }
}
