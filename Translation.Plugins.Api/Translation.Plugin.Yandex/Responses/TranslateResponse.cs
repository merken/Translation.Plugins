using System.Text.Json.Serialization;

namespace Translation.Plugin.Yandex.Responses
{
    public class TranslateResponse : DetectLanguageResponse
    {
        [JsonPropertyName("text")]
        public string[] Candidates { get; set; }
    }
}
