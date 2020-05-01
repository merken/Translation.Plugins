using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Translation.Plugin.Contract;

namespace Translation.Plugins.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslationController : ControllerBase
    {
        private readonly ILogger<TranslationController> _logger;
        private readonly IEnumerable<ITranslationPlugin> plugins;

        public TranslationController(ILogger<TranslationController> logger, IEnumerable<ITranslationPlugin> plugins)
        {
            _logger = logger;
            this.plugins = plugins;
        }

        [HttpGet]
        public async Task<string> Get([FromQuery] string text, [FromQuery] string source, [FromQuery] string target)
        {
            var builder = new StringBuilder();
            var request = new TranslationRequest
            {
                SourceLanguage = new TranslationLanguage { LanguageCode = source },
                TargetLanguage = new TranslationLanguage { LanguageCode = target },
                Text = text
            };

            foreach (var plugin in plugins)
                builder.AppendLine((await plugin.Translate(request)).TranslatedText);

            return builder.ToString();
        }
    }
}
