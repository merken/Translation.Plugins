using System.Text.Json.Serialization;

namespace Translation.Plugin.IBMWatson.DTO
{
    public class IBMWatsonTranslationRequest
    {
        [JsonPropertyName("text")]
        public string[] Text { get; set; }
        [JsonPropertyName("model_id")]
        public string ModelId { get; set; }
    }
}
