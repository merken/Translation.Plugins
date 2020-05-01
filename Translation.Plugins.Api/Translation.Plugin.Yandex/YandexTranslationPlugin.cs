using System;
using System.Net.Http;
using System.Threading.Tasks;
using Prise.Plugin;
using Translation.Plugin.Contract;
using Translation.Plugin.Yandex.Responses;

namespace Translation.Plugin.Yandex
{
    public class YandexTranslationPlugin : ITranslationPlugin
    {
        private const string DetectUrlFormat = "detext?key={0}&text={1}";
        private const string TranslateUrlFormat = "translate?key={0}&text={1}&lang={2}";

        // This is injected by the API Host application
        [PluginService(ProvidedBy = ProvidedBy.Host, ServiceType = typeof(PluginConfiguration))]
        private readonly PluginConfiguration pluginConfiguration;

        // This is provided by the YandexPluginBootstrapper
        [PluginService(ProvidedBy = ProvidedBy.Plugin, ServiceType = typeof(HttpClient))]
        private readonly HttpClient client;

        public async Task<TranslationResponse> Translate(TranslationRequest request)
        {
            var apiKey = pluginConfiguration.Values["YandexApiKey"];
            var sourceLanguage = request.SourceLanguage?.LanguageCode;

            if (!String.IsNullOrEmpty(sourceLanguage))
                sourceLanguage = ParseYandexLanguage(sourceLanguage);
            else
            {
                // Detect source language
                var detectLanguage = await DeserializeResponse<DetectLanguageResponse>(await client.GetAsync(String.Format(DetectUrlFormat, apiKey, request.Text)));
                sourceLanguage = detectLanguage.Language;
            }

            var targetLanguage = ParseYandexLanguage(request.TargetLanguage.LanguageCode);
            var languageParameter = $"{sourceLanguage}-{targetLanguage}";
            var translationResponse = await DeserializeResponse<TranslateResponse>(
                await client.GetAsync(String.Format(TranslateUrlFormat, apiKey, request.Text, languageParameter)));

            return new TranslationResponse
            {
                Accuracy = -1, // not supported using Yandex
                OriginalText = request.Text,
                SourceLanguage = request.SourceLanguage,
                TargetLanguage = request.TargetLanguage,
                TranslatedText = translationResponse.Candidates?[0] // Other results are not returned
            };
        }

        private string ParseYandexLanguage(string languageCode)
        {
            if (languageCode.Contains('-'))
                return languageCode.Split('-')[0];

            return languageCode;
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStreamAsync();
            return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(body, new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
        }
    }
}
