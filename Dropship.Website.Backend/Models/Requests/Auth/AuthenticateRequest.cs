using System.Text.Json.Serialization;

namespace Dropship.Website.Backend.Models.Requests.Auth
{
    public class AuthenticateRequest
    {
        /// <summary>
        ///     Can be email or username.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}