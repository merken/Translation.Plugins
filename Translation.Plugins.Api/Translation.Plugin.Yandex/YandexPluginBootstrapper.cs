using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace Translation.Plugin.Yandex
{
    [PluginBootstrapper(PluginType = typeof(YandexTranslationPlugin))]
    public class YandexPluginBootstrapper : IPluginBootstrapper
    {
        private const string YandexApi = "https://translate.yandex.net/api/v1.5/tr.json/";
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            // Configure the HttpClient required for the YandexTranslationPlugin
            return services.AddScoped<HttpClient>(s =>
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(YandexApi);
                return client;
            });
        }
    }
}
