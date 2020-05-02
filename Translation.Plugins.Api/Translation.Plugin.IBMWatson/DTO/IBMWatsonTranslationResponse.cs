using System.Text.Json.Serialization;

namespace Translation.Plugin.IBMWatson.DTO
{
    public class IBMWatsonTranslation
    {
        [JsonPropertyName("translation")]
        public string Translation { get; set; }
    }

    public class IBMWatsonTranslationResponse
    {
        [JsonPropertyName("translations")]
        public IBMWatsonTranslation[] Translations { get; set; }
    }
}
