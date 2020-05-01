using System.Text.Json.Serialization;

namespace Translation.Plugin.Yandex.Responses
{
    public class DetectLanguageResponse : YandexApiResponse
    {
        [JsonPropertyName("lang")]
        public string Language { get; set; }
    }
}
