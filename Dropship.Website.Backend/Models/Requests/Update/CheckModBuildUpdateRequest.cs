using System.Text.Json.Serialization;
using Dropship.Website.Backend.Database.DataTypes;

namespace Dropship.Website.Backend.Models.Requests.Update
{
    public class CheckModBuildUpdateRequest
    {
        [JsonPropertyName("gameVersion")] 
        public int GameVersion { get; set; }

        [JsonPropertyName("gamePlatform")] 
        public GamePlatform GamePlatform { get; set; }
        
        [JsonPropertyName("guids")] 
        public string[] Guids { get; set; }
    }
}