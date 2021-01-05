using System;
using System.Text.Json.Serialization;

namespace Dropship.Website.Backend.Models.Requests.ServerJoin
{
    public class CheckClientJoinRequest
    {
        [JsonPropertyName("userId")]
        public String UserId { get; set; }
        [JsonPropertyName("serverNonce")]
        public String ServerNonce { get; set; }
    }
}