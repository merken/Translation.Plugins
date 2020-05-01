using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Translation.Plugin.Microsoft.DTO
{

    public class MSCogneticServicesTranslationRequestItem
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class MSCogneticServicesTranslationLanguage
    {
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("score")]
        public decimal Score { get; set; }
    }

    public class MSCogneticServicesTranslation
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("to")]
        public string To { get; set; }
    }

    public class MSCogneticServicesTranslationResponseItem
    {
        [JsonProperty("detectedLanguage")]
        public MSCogneticServicesTranslationLanguage DetectedLanguage { get; set; }

        [JsonProperty("translations")]
        public MSCogneticServicesTranslation[] Translations { get; set; }
    }
}
