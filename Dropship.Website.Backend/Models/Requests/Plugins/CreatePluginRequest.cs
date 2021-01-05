using System.Text.Json.Serialization;
using Dropship.Website.Backend.Database.Enums;

namespace Dropship.Website.Backend.Models.Requests.Plugins
{
    public class CreatePluginRequest
    {
        [JsonPropertyName("guid")]
        public string Guid { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("serverDistroId")]
        public ServerDistro ServerDistroId { get; set; }
        
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
    }
}