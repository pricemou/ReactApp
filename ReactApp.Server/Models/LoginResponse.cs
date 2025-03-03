
namespace ReactApp.Server.Data
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public bool EstAdministrateur { get; set; } // Nouvelle propriété
    }
}
