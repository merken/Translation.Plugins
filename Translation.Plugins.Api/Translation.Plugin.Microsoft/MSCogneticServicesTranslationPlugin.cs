using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Prise.Plugin;
using Translation.Plugin.Contract;
using Translation.Plugin.Microsoft.DTO;

namespace Translation.Plugin.Microsoft
{
    [Plugin(PluginType = typeof(ITranslationPlugin))]
    public class MSCogneticServicesTranslationPlugin : ITranslationPlugin
    {
        private const string V3TranslationApiFormat = "translate?api-version=3.0&to={0}";

        // This is injected by the API Host application
        [PluginService(ProvidedBy = ProvidedBy.Host, ServiceType = typeof(IPluginConfigurationProvider), BridgeType = typeof(Translation.Plugins.Util.PluginConfigurationProviderBridge))]
        private readonly IPluginConfigurationProvider pluginConfigurationProvider;

        // This is provided by the YandexPluginBootstrapper
        [PluginService(ProvidedBy = ProvidedBy.Plugin, ServiceType = typeof(HttpClient))]
        private readonly HttpClient client; // .NET Core 2.1 HttpClient

        public async Task<TranslationResponse> Translate(TranslationRequest request)
        {
            var configuration = await this.pluginConfigurationProvider.ProvideForPluginKey("MSCogneticServices");
            var apiKey = configuration.Values["Ocp-Apim-Subscription-Key"];
            var region = configuration.Values["Ocp-Apim-Subscription-Region"];

            var requestPayload = CreateRequestBody(request.Text);
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");
            content.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);
            content.Headers.Add("Ocp-Apim-Subscription-Region", region);

            var response = await DeserializeResponse<MSCogneticServicesTranslationResponseItem[]>(await client.PostAsync(String.Format(V3TranslationApiFormat, request.TargetLanguage.LanguageCode), content));
            var translation = response[0]; // Only interested in first response

            return new TranslationResponse
            {
                Accuracy = translation.DetectedLanguage.Score * 100.0m, // not supported using Yandex
                OriginalText = request.Text,
                SourceLanguage = request.SourceLanguage,
                TargetLanguage = request.TargetLanguage,
                TranslatedText = translation.Translations?[0]?.Text // Other results are not returned
            };
        }

        private MSCogneticServicesTranslationRequestItem[] CreateRequestBody(string text)
        {
            // creates an array of 1 item
            return new MSCogneticServicesTranslationRequestItem[1] { new MSCogneticServicesTranslationRequestItem { Text = text } };
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(body);
        }
    }
}
