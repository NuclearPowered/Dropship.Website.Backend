using System.Text.Json.Serialization;

namespace Dropship.Website.Backend.Models.Requests.Auth
{
    public class RegisterRequest
    {
        [JsonPropertyName("username")] 
        public string Username { get; set; }

        [JsonPropertyName("email")] 
        public string Email { get; set; }

        [JsonPropertyName("password")] 
        public string Password { get; set; }
    }
}