using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prise.Proxy;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            {
                var pluginType = GetPluginName(plugin);
                var pluginResult = await plugin.Translate(request);

                builder.AppendLine($"{pluginResult.TranslatedText} : {pluginType}");
            }

            return builder.ToString();
        }

        private string GetPluginName(ITranslationPlugin plugin)
        {
            var field = typeof(PriseProxy<ITranslationPlugin>).GetField("remoteObject", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            return (field.GetValue(plugin) as object).ToString();
        }
    }
}
