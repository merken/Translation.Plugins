using System.Text.Json.Serialization;

namespace Translation.Plugin.Yandex.Responses
{
    /// <summary>
    /// Base response class for any Yandex response
    /// </summary>
    public abstract class YandexApiResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
