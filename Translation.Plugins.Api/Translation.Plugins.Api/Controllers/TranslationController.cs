using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prise.Proxy;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Translation.Plugin.Contract;

namespace Translation.Plugins.Api.Controllers
{
    public class TranslationApiResponse
    {
        [JsonPropertyName("translation")]
        public string Translation { get; set; }

        [JsonPropertyName("plugin")]
        public string Plugin { get; set; }
    }

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
        public async Task<TranslationApiResponse[]> Get([FromQuery] string text, [FromQuery] string source, [FromQuery] string target)
        {
            var responses = new List<TranslationApiResponse>();
            var request = new TranslationRequest
            {
                SourceLanguage = new TranslationLanguage { LanguageCode = source },
                TargetLanguage = new TranslationLanguage { LanguageCode = target },
                Text = text
            };

            foreach (var plugin in plugins)
            {
                var pluginName = GetPluginName(plugin);
                var pluginResult = await plugin.Translate(request);

                responses.Add(new TranslationApiResponse
                {
                    Plugin = pluginName,
                    Translation = pluginResult.TranslatedText
                });
            }

            return responses.ToArray();
        }

        private string GetPluginName(ITranslationPlugin plugin)
        {
            var field = typeof(PriseProxy<ITranslationPlugin>).GetField("remoteObject", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var pluginName = (field.GetValue(plugin) as object).ToString();
            var removeNamespace = pluginName.Replace("Translation.Plugin.", string.Empty);
            return removeNamespace.Split('.')[0];
        }
    }
}
