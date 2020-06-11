using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;
using System;
using System.Net.Http;

namespace Translation.Plugin.IBMWatson
{
    [PluginBootstrapper(PluginType = typeof(IBMWatsonTranslationPlugin))]
    public class IBMWatsonTranslationPluginBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            return services
                .AddScoped<ISettingsService, SettingsService>()
                .AddScoped<HttpClient>(s =>
                {
                    var client = new HttpClient();
                    return client;
                })
            ;
        }
    }
}
