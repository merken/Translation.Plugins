using Prise.Plugin;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Translation.Plugin.Contract;
using Translation.Plugin.Systran.DTO;

namespace Translation.Plugin.Systran
{
    [Plugin(PluginType = typeof(ITranslationPlugin))]
    public class SystranTranslationPlugin : ITranslationPlugin
    {
        private const string TranslateUrlFormat = "translate?key={0}&input={1}&source={2}&target={3}";

        // This is injected by the API Host application
        [PluginService(ProvidedBy = ProvidedBy.Host, ServiceType = typeof(IPluginConfigurationProvider), BridgeType = typeof(Translation.Plugins.Util.PluginConfigurationProviderBridge))]
        private readonly IPluginConfigurationProvider pluginConfigurationProvider;

        // This is provided by the SystranTranslationPluginBootstrapper
        [PluginService(ProvidedBy = ProvidedBy.Plugin, ServiceType = typeof(HttpClient))]
        private readonly HttpClient client;

        public async Task<TranslationResponse> Translate(TranslationRequest request)
        {
            var settings = await this.pluginConfigurationProvider.ProvideForPluginKey("Systran");
            var apiKey = settings.Values["ApiKey"];
            var sourceLanguage = request.SourceLanguage?.LanguageCode;
            var targetLanguage = request.TargetLanguage?.LanguageCode;
            var url = String.Format(TranslateUrlFormat, apiKey, request.Text, sourceLanguage, targetLanguage);
            var response = await this.client.GetAsync(url);
            var output = await DeserializeResponse<SystranTranslationResponse>(response);

            return new TranslationResponse
            {
                Accuracy = -1, // not supported using Yandex
                OriginalText = request.Text,
                SourceLanguage = request.SourceLanguage,
                TargetLanguage = request.TargetLanguage,
                TranslatedText = output.Outputs?[0].Output // Other results are not returned
            };
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(body, new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
        }
    }
}
