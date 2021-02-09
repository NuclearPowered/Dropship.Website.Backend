using System.Text.Json.Serialization;

namespace Dropship.Website.Backend.Models.Requests.Plugins
{
    public class UpdatePluginRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

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