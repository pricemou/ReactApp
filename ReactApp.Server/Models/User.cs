using System.Text.Json.Serialization;

namespace ReactApp.Server.Models
{
    public class User
    {
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }
    }
}
