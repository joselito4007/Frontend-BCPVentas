using System.Text.Json.Serialization;

namespace Frontend_BCPVentas.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Pwd { get; set; }
        public class Access
        {
            [JsonPropertyName("token")]
            public string Token { get; set; }
        }
    }
}
