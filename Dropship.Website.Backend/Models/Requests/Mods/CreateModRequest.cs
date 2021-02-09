using System.Text.Json.Serialization;

namespace Dropship.Website.Backend.Models.Requests.Mods
{
    public class CreateModRequest
    {
        [JsonPropertyName("guid")]
        public string Guid { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("markdownDescription")]
        public string MarkdownDescription { get; set; }
        
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
    }
}