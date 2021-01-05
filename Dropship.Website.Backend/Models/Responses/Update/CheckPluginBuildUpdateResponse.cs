using System.Text.Json.Serialization;
using Dropship.Website.Backend.Database.Entities;

namespace Dropship.Website.Backend.Models.Responses.Update
{
    public class CheckPluginBuildUpdateResponse
    {
        [JsonPropertyName("guid")] 
        public string Guid { get; set; }

        [JsonPropertyName("pluginBuild")] 
        public PluginBuildEntity PluginBuild { get; set; }
    }
}