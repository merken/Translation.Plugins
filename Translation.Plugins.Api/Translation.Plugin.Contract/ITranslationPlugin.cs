using System.Threading.Tasks;

namespace Translation.Plugin.Contract
{
    public class TranslationLanguage
    {
        public string LanguageCode { get; set; }
        public string LanguageDescription { get; set; }
    }

    public class TranslationRequest
    {
        public string Text { get; set; }
        public TranslationLanguage SourceLanguage { get; set; }
        public TranslationLanguage TargetLanguage { get; set; }
    }

    public class TranslationResponse
    {
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
        public decimal Accuracy { get; set; }
        public TranslationLanguage SourceLanguage { get; set; }
        public TranslationLanguage TargetLanguage { get; set; }

    }

    public interface ITranslationPlugin
    {
        Task<TranslationResponse> Translate(TranslationRequest request);
    }
}
