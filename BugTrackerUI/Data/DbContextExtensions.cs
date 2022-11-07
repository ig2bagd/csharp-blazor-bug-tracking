using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerUI.Data
{
   // https://stackoverflow.com/a/61085939/3494566
   public static class DbContextExtensions
   {
      public static IList<T> SqlQuery<T>(this DbContext context, string sql, params object[] parameters) where T : class
      {
         using (var dbcontext = new ContextForQueryType<T>(context.Database.GetDbConnection()))
         {
            return dbcontext.Set<T>().FromSqlRaw(sql, parameters).AsNoTracking().ToList();
         }
      }

      public static async Task<IList<T>> SqlQueryAsync<T>(this DbContext context, string sql, params object[] parameters) where T : class
      {
         using (var dbcontext = new ContextForQueryType<T>(context.Database.GetDbConnection()))
         {
            return await dbcontext.Set<T>().FromSqlRaw(sql, parameters).AsNoTracking().ToListAsync();
         }
      }

      private class ContextForQueryType<T> : DbContext where T : class
      {
         private readonly System.Data.Common.DbConnection connection;

         public ContextForQueryType(System.Data.Common.DbConnection connection)
         {
            this.connection = connection;
         }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
            optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());

            base.OnConfiguring(optionsBuilder);
         }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
            modelBuilder.Entity<T>().HasNoKey();
            base.OnModelCreating(modelBuilder);
         }
      }
   }
}
