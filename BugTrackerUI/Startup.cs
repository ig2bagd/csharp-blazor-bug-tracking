using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BugTrackerUI.Services;
using BugTrackerUI.Data;
using Microsoft.EntityFrameworkCore;
using Fluxor;
using System.Net.Http;

namespace BugTrackerUI
{
   public class Startup
   {
      public IConfiguration Configuration { get; }
      public IWebHostEnvironment _env { get; }
      public Startup(IConfiguration configuration, IWebHostEnvironment env)
      {
         Configuration = configuration;
         _env = env;
      }


      // This method gets called by the runtime. Use this method to add services to the container.
      // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
      public void ConfigureServices(IServiceCollection services)
      {
         string baseUri = Configuration.GetValue<string>("BaseUri");

         services.AddMvc();			               // configures both Razor Pages and MVC or individually as shown below:		
         //services.AddControllersWithViews();
         //services.AddRazorPages();

         services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = _env.IsDevelopment(); });
         services.AddTelerikBlazor();

         // register an instance of type IHttpClientFactory: Basic HTTPClient
         //services.AddHttpClient();         
         
         /* 
         // Named client
         //services.AddHttpClient("AdminApi", client => client.BaseAddress = new Uri(BaseUri));
         services.AddHttpClient("AdminApi", client => 
         {
            client.BaseAddress = new Uri(baseUri);
         })
         .ConfigurePrimaryHttpMessageHandler(() => 
         {
            return new HttpClientHandler()
            {
               UseDefaultCredentials = false,
               Credentials = System.Net.CredentialCache.DefaultCredentials,
               AllowAutoRedirect = true
            };
         });
         */
         // Typed client: registers NorthwindService as a transient service
         services.AddHttpClient<INorthwindService, NorthwindService>( client =>
         {
            client.BaseAddress = new Uri(baseUri);
         })
         .SetHandlerLifetime(TimeSpan.FromMinutes(5))       // Default is 2 mins
         //.ConfigurePrimaryHttpMessageHandler(() =>
         //{
         //   return new HttpClientHandler()
         //   {
         //      UseDefaultCredentials = false,
         //      Credentials = System.Net.CredentialCache.DefaultCredentials,
         //      AllowAutoRedirect = true
         //   };
         //});
         .ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler()
            {
               UseDefaultCredentials = false,
               Credentials = System.Net.CredentialCache.DefaultCredentials,
               AllowAutoRedirect = true
            });

         //services.AddTransient<IUrbanAreaService, UrbanAreaService>();

         services.AddSingleton<IBugService, BugService>();
         services.AddScoped<JsConsole>();
         services.AddScoped<IOrderService, OrderService>();
         services.AddScoped<IProductService, ProductService>();
         services.AddScoped<IdentityInformation>();

         services.AddDbContext<AppDbContext>(o =>
         {
            o.UseSqlServer(Configuration.GetConnectionString("AppDb"));
            o.LogTo(Console.WriteLine);
         });

         // Add Fluxor
         services.AddFluxor(o =>
            o.ScanAssemblies(typeof(Startup).Assembly)
             .UseReduxDevTools()
             .UseRouting()
         );
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }

         app.UseHttpsRedirection();
         app.UseStaticFiles();

         app.UseRouting();

         app.UseEndpoints(endpoints =>
         {
            //endpoints.MapRazorPages();
            endpoints.MapControllers();      // To serve Controllers & APIs

            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
         });
      }
   }
}
