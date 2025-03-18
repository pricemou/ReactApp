using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class SupprimerUtilisateurClientRequest
    {
        public string Email { get; set; }
    }
}