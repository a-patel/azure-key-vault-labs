#region Imports
using AzureKeyVaultLabs.Demo.Extensions;
using AzureKeyVaultLabs.Demo.Models;
using AzureKeyVaultLabs.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#endregion

namespace AzureKeyVaultLabs.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //// add a Settings model to the service container, which takes its values from the applications configuration.
            services.Configure<Settings>(Configuration.GetSection("Settings"));

            services.AddScoped<IAzureKeyVaultService, AzureKeyVaultService>();

            services.AddControllers();

            services.AddCustomSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCustomSwagger();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
