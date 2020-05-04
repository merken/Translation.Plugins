using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;
using System;
using System.Net.Http;

namespace Translation.Plugin.Systran
{
    [PluginBootstrapper(PluginType = typeof(SystranTranslationPlugin))]
    public class SystranTranslationPluginBootstrapper : IPluginBootstrapper
    {
        private const string SystranApi = "https://api-platform.systran.net/translation/text/";
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            return services.AddScoped<HttpClient>(s =>
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(SystranApi);
                return client;
            });
        }
    }
}
