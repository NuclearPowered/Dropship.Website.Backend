using System.Text.Json.Serialization;
using Dropship.Website.Backend.Database.DataTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dropship.Website.Backend.Models.Requests.ModBuilds
{
    public class CreateModBuildRequest
    {
        /// <summary>
        ///     Foreign key Id for the ModEntity that the build is for.
        /// </summary>
        [JsonPropertyName("modId")]
        public int ModId { get; set; }

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
        ///     Game Version that the mod build is for.
        /// </summary>
        [JsonPropertyName("gameVersion")]
        public int GameVersion { get; set; }
        
        /// <summary>
        ///     Game Platform that the mod build is for.
        /// </summary>
        [JsonPropertyName("gamePlatform")]
        public GamePlatform GamePlatform { get; set; }

        /// <summary>
        ///     Contains the Download URL for the mod.
        /// </summary>
        [JsonPropertyName("downloadUrl")]
        public string DownloadUrl { get; set; }
        
        /// <summary>
        ///     The ModBuildEntity dependencies of the mod
        /// </summary>
        [JsonPropertyName("dependencyIds")]
        public int[] DependencyIds { get; set; }
    }
}