using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class Utilisateur
    {
        [Key]
        [Column("id_utilisateur")]
        public int id_utilisateur { get; set; }

        [Column("nom_utilisateur")]
        public string nom_utilisateur { get; set; }

        [Column("mot_de_passe")]
        public string mot_de_passe { get; set; }

        [Column("email")]
        public string email { get; set; }

        [Column("est_administrateur")]
        public bool est_administrateur { get; set; }

    }

}