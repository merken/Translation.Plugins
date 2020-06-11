using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace Translation.Plugin.Yandex
{
    [PluginBootstrapper(PluginType = typeof(YandexTranslationPlugin))]
    public class YandexTranslationPluginBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            // Configure the HttpClient required for the YandexTranslationPlugin
            return services.AddScoped<HttpClient>(s =>
            {
                var client = new HttpClient();
                var config = s.GetRequiredService<IConfiguration>();
                var baseUrl = config["Yandex:BaseUrl"];
                client.BaseAddress = new Uri(baseUrl);
                return client;
            });
        }
    }
}
