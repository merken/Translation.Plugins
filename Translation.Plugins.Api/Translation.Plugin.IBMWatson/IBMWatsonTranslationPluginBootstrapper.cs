using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;
using System;
using System.Net.Http;

namespace Translation.Plugin.IBMWatson
{
    [PluginBootstrapper(PluginType = typeof(IBMWatsonTranslationPlugin))]
    public class IBMWatsonTranslationPluginBootstrapper : IPluginBootstrapper
    {
        private const string IBMWatsonTranslationApi = "https://api.eu-gb.language-translator.watson.cloud.ibm.com/instances/10c001b3-291d-49b5-8ef9-89407e6be57d/v3/";

        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            return services.
                AddScoped<HttpClient>(s =>
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(IBMWatsonTranslationApi);
                    return client;
                })
                .AddScoped<ISettingsService, SettingsService>()
            ;
        }
    }
}
