using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prise.Plugin;
using Translation.Plugin.Contract;
using Translation.Plugin.Microsoft.DTO;

namespace Translation.Plugin.Microsoft
{

    public class MSCogneticServicesTranslationPlugin : ITranslationPlugin
    {
        private const string V3TranslationApiFormat = "?api-version=3.0&to={0}";

        // This is injected by the API Host application
        [PluginService(ProvidedBy = ProvidedBy.Host, ServiceType = typeof(PluginConfiguration))]
        private readonly PluginConfiguration pluginConfiguration;

        // This is provided by the YandexPluginBootstrapper
        [PluginService(ProvidedBy = ProvidedBy.Plugin, ServiceType = typeof(HttpClient))]
        private readonly HttpClient client; // .NET Core 2.1 HttpClient

        public async Task<TranslationResponse> Translate(TranslationRequest request)
        {
            var apiKey = pluginConfiguration.Values["Ocp-Apim-Subscription-Key"];
            var region = pluginConfiguration.Values["Ocp-Apim-Subscription-Region"];

            var requestPayload = CreateRequestBody(request.Text);
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");
            content.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);
            content.Headers.Add("Ocp-Apim-Subscription-Region", region);

            var response = await DeserializeResponse<MSCogneticServicesTranslationResponseItem[]>(await client.PostAsync("translate", content));
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

        private Newtonsoft.Json.Linq.JArray CreateRequestBody(string text)
        {
            // creates an array of 1 item
            return new Newtonsoft.Json.Linq.JArray(new MSCogneticServicesTranslationRequestItem { Text = text });
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(body);
        }
    }
}
