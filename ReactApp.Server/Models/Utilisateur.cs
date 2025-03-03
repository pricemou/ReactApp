using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class Utilisateur
    {
        [Key]
        [Column("id_utilisateur")]
        public int id_utilisateur  { get; set; }

        public string nom_utilisateur  { get; set; }
        public string mot_de_passe  { get; set; }
        public string email  { get; set; }
        public bool est_administrateur  { get; set; }
        public DateTime date_creation  { get; set; }
        public DateTime? derniere_connexion  { get; set; }
    }
}