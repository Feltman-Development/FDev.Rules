using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FDEV.Rules.Demo.UI.DemonstrationSite.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FDEV.Rules.Demo.UI.DemonstrationSite
{
    /// <summary>
    /// Where all the magic starts...
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Los Constructus Maximus
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// The Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940 
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
        }

        /// <summary>
        /// This method gets called by the runtime and lays down the (HTTP) pipeline.
        /// This is where we intersect, overrule, proxy and whatever we can do to take control of how our app functions.
        /// </summary>
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

            //NOTE: Below is some of the voodoo we are trying to use a shared MVC projects, some adjustments might be needed
            // Write streamlined request completion events, instead of the more verbose ones from the framework.
            // To use the default framework request logging instead, remove this line and set the "Microsoft"
            // level in appsettings.json to "Information".
            app.UseSerilogRequestLogging();

            app.UseRouting();
            
            //NOTE: Below is some of the voodoo we are trying to use a shared MVC projects, some adjustments might be needed
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                //NOTE: Below is some of the voodoo we are trying to use a shared MVC projects, some adjustments might be needed
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
        }
    }
}
