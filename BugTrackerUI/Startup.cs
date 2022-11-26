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
using Newtonsoft.Json.Serialization;
using Fluxor;
using System.Net.Http;
using System.Net.Http.Headers;

using Polly;
using Polly.Extensions.Http;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

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

         services.AddMvc();			               // This method has all the features (Web API, MVC, and Razor Pages).		
         //services.AddControllers();              // Web API
         //services.AddControllersWithViews();     // MVC
         //services.AddRazorPages();               // Razor Pages

         // https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/server?view=aspnetcore-6.0
         // https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/signalr?view=aspnetcore-6.0#circuit-handler-options-for-blazor-server-apps-1
         services.AddServerSideBlazor()
            .AddCircuitOptions(option => 
            { 
               option.DetailedErrors = _env.IsDevelopment(); 
            })
            .AddHubOptions(options =>
             {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);        // (default: 30 seconds)
                options.HandshakeTimeout = TimeSpan.FromSeconds(30);             // (default: 15 seconds)
             });
         ;
         services.AddTelerikBlazor();

         // register an instance of type IHttpClientFactory: Basic HTTPClient
         //services.AddHttpClient();         

         // Named client
         //services.AddHttpClient("AdminApi", client => client.BaseAddress = new Uri(BaseUri));

         // Typed client: registers NorthwindService as a transient service
         services.AddHttpClient<INorthwindService, NorthwindService>(client =>
        {
           client.BaseAddress = new Uri(baseUri);
            //client.DefaultRequestHeaders.Clear();
            //client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
         })
         .SetHandlerLifetime(TimeSpan.FromMinutes(5))       // Default is 2 mins

         .AddPolicyHandler(GetRetryPolicy())                                                                         // Polly
         //.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)))            // Polly
         .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(new[]                                                 // Polly
             {
                 TimeSpan.FromSeconds(1),
                 TimeSpan.FromSeconds(5)
             }))
         .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)))                       // Polly

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
         services.AddScoped<AuthenticationStateProvider, IdentityValidationProvider<IdentityUser>>();

         // ==> Use in razor component: @using Microsoft.EntityFrameworkCore
         //                             @inject IDbContextFactory<AppDbContext> ctxFactory
         //                             using(var ctx = ctxFactory.CreateDbContext()) { ... }
         services.AddDbContextFactory<AppDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("AppDb")));         // Use DbContextFactory in the Blazor part; EF Core 5+

         services.AddDbContext<AppDbContext>(o =>                                                                             // Use DbContext in controllers or Razor pages
         {
            o.UseSqlServer(Configuration.GetConnectionString("AppDb"));
            o.LogTo(Console.WriteLine);
         });

         // Register the Swagger generator, defining 1 or more Swagger documents
         services.AddSwaggerGen(c =>
         {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
         });


         // Override the limit (32kb) SignalR can process in a single go
         // https://medium.com/it-dead-inside/lets-learn-blazor-file-streaming-with-jsinterop-240cfc133452
         // https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.signalr.huboptions.enabledetailederrors
         // https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-6.0&tabs=dotnet#configure-server-options
         services.AddSignalR(hubOptions =>
         {
            hubOptions.EnableDetailedErrors = true;
            //hubOptions.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
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
         // Call UsePathBase first in the app's request processing pipeline
         // https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/?view=aspnetcore-3.1&tabs=visual-studio#app-base-path-2
         //app.UsePathBase("/CoolApp");

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

         // Enable middleware to serve generated Swagger as a JSON endpoint.
         app.UseSwagger();

         // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
         // specifying the Swagger JSON endpoint.
         app.UseSwaggerUI(c =>
         {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
         });

         app.UseRouting();
         // https://www.tektutorialshub.com/asp-net-core/asp-net-core-endpoint-routing/
         app.UseEndpoints(endpoints =>
         {
            //endpoints.MapRazorPages();
            endpoints.MapControllers();      // To serve Controllers & APIs

            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
         });

      }


      // https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
      static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
      {
         return HttpPolicyExtensions
             .HandleTransientHttpError()
             .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
             .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
      }


   }
}
