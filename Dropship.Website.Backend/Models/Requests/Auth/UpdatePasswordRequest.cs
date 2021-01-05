using System.Text.Json.Serialization;

namespace Dropship.Website.Backend.Models.Requests.Auth
{
    public class UpdatePasswordRequest
    {
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}