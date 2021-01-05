using System.Text.Json.Serialization;

namespace Dropship.Website.Backend.Models.Requests.ServerList
{
    public class CreateUpdateServerRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("ipaddress")]
        public int IpAddress { get; set; }

        [JsonPropertyName("port")]
        public ushort Port { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
    }
}