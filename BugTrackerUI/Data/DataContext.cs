using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerUI.Data
{
   public class DataContext : IdentityDbContext
   {
      public DataContext(DbContextOptions options) : base(options)
      {

      }
   }
}
