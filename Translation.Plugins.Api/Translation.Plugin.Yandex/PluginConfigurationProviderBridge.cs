using Prise.PluginBridge;
using System.Threading.Tasks;
using Translation.Plugin.Contract;

namespace Translation.Plugins.Util
{
    public class PluginConfigurationProviderBridge : IPluginConfigurationProvider
    {
        private readonly object hostService;
        public PluginConfigurationProviderBridge(object hostService)
        {
            this.hostService = hostService;
        }

        public Task<PluginConfiguration> ProvideForPluginKey(string pluginKey)
        {
            var methodInfo = typeof(IPluginConfigurationProvider).GetMethod(nameof(ProvideForPluginKey));
            return PrisePluginBridge.Invoke(this.hostService, methodInfo, pluginKey) as Task<PluginConfiguration>;
        }
    }
}
