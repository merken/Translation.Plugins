using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Translation.Plugin.Contract;

namespace Translation.Plugins.Api.PriseCustomizations
{
    public class AppSettingsPluginConfigurationProvider : IPluginConfigurationProvider
    {
        private readonly IConfiguration configuration;
        public AppSettingsPluginConfigurationProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<PluginConfiguration> ProvideForPluginKey(string pluginKey)
        {
            var values = this.configuration.GetSection(pluginKey).Get<Dictionary<string, string>>();
            return new PluginConfiguration
            {
                Values = values
            };
        }
    }
}
