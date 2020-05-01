using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prise;
using Prise.AssemblyScanning.Discovery;
using Translation.Plugin.Contract;
using Translation.Plugins.Api.PriseCustomizations;

namespace Translation.Plugins.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddPrise<ITranslationPlugin>(config =>
            {
                config.WithDefaultOptions(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"))
                .ScanForAssemblies(composer =>
                            composer.UseDiscovery())
                .UseHostServices(services, new[] { typeof(IConfiguration) })
                .ConfigureSharedServices(sharedServices =>
                {
                    sharedServices.AddScoped<IPluginConfigurationProvider, AppSettingsPluginConfigurationProvider>();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
