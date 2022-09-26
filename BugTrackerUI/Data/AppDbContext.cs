using Microsoft.EntityFrameworkCore;

namespace BugTrackerUI.Data
{
   public class AppDbContext : DbContext
   {
      public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
      public DbSet<Employee> Employees { get; set; }
      public DbSet<Customer> Customers { get; set; }
   }
}
