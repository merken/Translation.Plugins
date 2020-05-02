using Prise.Plugin;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Translation.Plugin.Contract;
using Translation.Plugin.IBMWatson.DTO;

namespace Translation.Plugin.IBMWatson
{
    [Plugin(PluginType = typeof(ITranslationPlugin))]
    public class IBMWatsonTranslationPlugin : ITranslationPlugin
    {
        private const string V3TranslationApi = "translate?version=2018-05-01";

        // This is injected by the API Host application
        [PluginService(ProvidedBy = ProvidedBy.Plugin, ServiceType = typeof(ISettingsService))]
        private readonly ISettingsService settingsService;

        // This is provided by the YandexPluginBootstrapper
        [PluginService(ProvidedBy = ProvidedBy.Plugin, ServiceType = typeof(HttpClient))]
        private readonly HttpClient client; // .NET Standard 2.1 HttpClient

        public async Task<TranslationResponse> Translate(TranslationRequest request)
        {
            var settings = await this.settingsService.GetSettings();

            var byteArray = new UTF8Encoding().GetBytes($"apikey:{settings.ApiKey}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var ibmRequest = new IBMWatsonTranslationRequest
            {
                Text = new[] { request.Text },
                ModelId = $"{request.SourceLanguage.LanguageCode}-{request.TargetLanguage.LanguageCode}"
            };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(ibmRequest), Encoding.UTF8, "application/json");

            var response = await this.client.PostAsync(V3TranslationApi, content);
            var translationsResponse = await DeserializeResponse<IBMWatsonTranslationResponse>(response);
            return new TranslationResponse
            {
                OriginalText = request.Text,
                SourceLanguage = request.SourceLanguage,
                TargetLanguage = request.TargetLanguage,
                TranslatedText = translationsResponse.Translations?[0]?.Translation // Other results are not returned
            };
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(body, new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
        }
    }
}
