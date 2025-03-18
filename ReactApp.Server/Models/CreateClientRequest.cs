using System.Text.Json.Serialization;

namespace ReactApp.Server.Models
{
    public class CreateClientRequest
    {
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
    }

}
