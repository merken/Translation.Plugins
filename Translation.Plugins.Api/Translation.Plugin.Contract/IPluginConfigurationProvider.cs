using System.Threading.Tasks;

namespace Translation.Plugin.Contract
{
    public interface IPluginConfigurationProvider
    {
        Task<PluginConfiguration> ProvideForPluginKey(string pluginKey);
    }
}
