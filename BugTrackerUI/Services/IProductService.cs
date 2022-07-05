using BugTrackerUI.Data;
using System.Collections.Generic;

namespace BugTrackerUI.Services
{
    public interface IProductService
    {
        List<Product> GetTopSellingProducts();
    }
}
