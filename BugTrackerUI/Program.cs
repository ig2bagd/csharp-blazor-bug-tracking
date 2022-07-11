using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BugTrackerUI
{
   public class Program
   {
      public static void Main(string[] args)
      {
         var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", false, true)
             //.AddJsonFile($"appsettings.Development.json", optional: true)
             .Build();

         Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(configuration)
             .CreateLogger();

         /*
         Log.Logger = new LoggerConfiguration()
         .Enrich.FromLogContext()
         .WriteTo.Console()
         .WriteTo.File("site.log", rollingInterval: RollingInterval.Day)
         //.WriteTo.File("logs/log.txt")
         //.WriteTo.File(@"C:\logs\log.txt",
         //        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
         //        rollingInterval: RollingInterval.Day,
         //        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
         .CreateLogger();
         */

         try
         {
            Log.Information("Starting web host");
            CreateHostBuilder(args).Build().Run();
         }
         catch (Exception ex)
         {

            Log.Fatal(ex, "Host terminated unexpectedly");
         }
         finally
         {
            Log.CloseAndFlush();
         }

      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .UseSerilog()
              //.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
              .ConfigureWebHostDefaults(webBuilder =>
              {
                 //https://stackoverflow.com/questions/70083598/site-css-not-found-in-net-6-web-app-with-custom-environment
                 //https://stackoverflow.com/questions/66430714/when-aspnetcore-environment-changed-to-anything-but-development-blazor-app-d
                 webBuilder.UseStaticWebAssets();     // may be needed when ASPNETCORE_ENVIRONMENT is NOT set to Development
                 webBuilder.UseStartup<Startup>();
              });
   }
}
