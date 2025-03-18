using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class UtilisateurClientRequest
    {
        public Utilisateur Utilisateur { get; set; }
        public Client Client { get; set; }
    }


}