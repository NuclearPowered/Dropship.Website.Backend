using System.Text.Json.Serialization;

namespace Dropship.Website.Backend.Models.Requests.PluginBuilds
{
    public class CreatePluginBuildRequest
    {
        /// <summary>
        ///     Foreign key Id for the PluginId that the build is for.
        /// </summary>
        [JsonPropertyName("pluginId")]
        public int PluginId { get; set; }

        /// <summary>
        ///     Human readable semver version string.
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        ///     FileName, purely for UI purposes.
        /// </summary>
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        /// <summary>
        ///     Contains the Download URL for the mod.
        /// </summary>
        [JsonPropertyName("downloadUrl")]
        public string DownloadUrl { get; set; }
        
        /// <summary>
        ///     The PluginBuildEntity dependencies of the mod
        /// </summary>
        [JsonPropertyName("dependencyIds")]
        public int[] DependencyIds { get; set; }
    }
}