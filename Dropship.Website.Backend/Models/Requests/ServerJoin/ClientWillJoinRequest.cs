using System;
using System.Text.Json.Serialization;

namespace Dropship.Website.Backend.Models.Requests.ServerJoin
{
    public class ClientWillJoinRequest
    {
        [JsonPropertyName("serverNonce")]
        public String ServerNonce { get; set; }
    }
}