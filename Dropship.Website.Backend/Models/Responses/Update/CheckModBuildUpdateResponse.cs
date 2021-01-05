using System.Text.Json.Serialization;
using Dropship.Website.Backend.Database.Entities;

namespace Dropship.Website.Backend.Models.Responses.Update
{
    public class CheckModBuildUpdateResponse
    {
        [JsonPropertyName("guid")] 
        public string Guid { get; set; }

        [JsonPropertyName("modBuild")] 
        public ModBuildEntity ModBuild { get; set; }
    }
}