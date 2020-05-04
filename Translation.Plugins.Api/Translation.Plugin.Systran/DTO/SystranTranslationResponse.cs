using System.Text.Json.Serialization;

namespace Translation.Plugin.Systran.DTO
{
    public class SystranTranslationResponse
    {
        [JsonPropertyName("outputs")]
        public SystranTranslationOutput[] Outputs { get; set; }
    }

    public class SystranTranslationOutput
    {
        [JsonPropertyName("output")]
        public string Output { get; set; }
    }
}
