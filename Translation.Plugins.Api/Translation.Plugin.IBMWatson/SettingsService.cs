using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Translation.Plugin.IBMWatson
{
    public class Settings
    {
        [JsonPropertyName("baseUrl")]
        public string BaseUrl { get; set; }
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; }
    }

    public interface ISettingsService
    {
        Task<Settings> GetSettings();
    }

    public class SettingsService : ISettingsService
    {
        public async Task<Settings> GetSettings()
        {
            var isDeployedAsPackage = false;
            var filePath = Path.Combine(GetLocalExecutionPath(), "Plugins", "Translation.Plugin.IBMWatson", "IBM.settings.json");

            // If this plugin was deployed as a NuGet package, the IBM.settings.json will be extracted into the default extraction dir
            // We cannot assume the targetplatform is netstandard2.1, so we need to search for this file using the Directory.GetFiles API
            if (isDeployedAsPackage)
            {
                var searchPattern = "IBM.settings.json";
                var searchDirectory = Path.Combine(GetLocalExecutionPath(), "Plugins", "_extracted", "Translation.Plugin.IBMWatson", "lib");
                filePath = Directory.GetFiles(searchDirectory, searchPattern, SearchOption.AllDirectories).FirstOrDefault();
            }

            if (!File.Exists(filePath))
                return null;

            return await System.Text.Json.JsonSerializer.DeserializeAsync<Settings>(File.OpenRead(filePath), new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
        }

        private string GetLocalExecutionPath()
        {
            // This is the location where the HOST application is executing, the bin/Debug/netcoreapp3.1 directory of the API
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
