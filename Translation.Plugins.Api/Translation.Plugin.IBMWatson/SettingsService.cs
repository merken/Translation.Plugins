using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Translation.Plugin.IBMWatson
{
    public class Settings
    {
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
            var filePath = Path.Combine(GetLocalExecutionPath(), "Plugins", "Translation.Plugin.IBMWatson", $"IBM.settings.json");
            if (!File.Exists(filePath))
                return null;
            return await System.Text.Json.JsonSerializer.DeserializeAsync<Settings>(File.OpenRead(filePath), new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
        }

        private string GetLocalExecutionPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
