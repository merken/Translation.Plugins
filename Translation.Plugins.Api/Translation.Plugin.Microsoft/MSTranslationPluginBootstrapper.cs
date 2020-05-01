using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace Translation.Plugin.Microsoft
{
    [PluginBootstrapper(PluginType =typeof(MSCogneticServicesTranslationPlugin))]
    public class MSTranslationPluginBootstrapper : IPluginBootstrapper
    {
        private const string MSCogneticServicesApi = "https://api.cognitive.microsofttranslator.com/translate";
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            // Configure the HttpClient required for the YandexTranslationPlugin
            return services.AddScoped<HttpClient>(s => // This is a .NET Core 2.1 HttpClient
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(MSCogneticServicesApi);
                return client;
            });
        }
    }
}
