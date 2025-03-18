using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    [Table("Vue_Utilisateur_Activites")] // Associe la classe à la vue SQL
    public class VueUtilisateurActivites
    {
        [Key]
        [Column("id_utilisateur")]
        public int Id_Utilisateur { get; set; }

        [Column("nom_utilisateur")]
        public string Nom_Utilisateur { get; set; }
        	
        [Column("mot_de_passe")]
        public string Mot_de_passe { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("nombre_decodeurs")]
        public int Nombre_Decodeurs { get; set; }

        [Column("nombre_operations")]
        public int Nombre_Operations { get; set; }

        [Column("est_administrateur")]
        public bool EstAdministrateur { get; set; }

        [Column("nom_client")]
        public string Nom_Client { get; set; }
        
        [Column("adresse_client")]
        public string Adresse_Client { get; set; }
    }
}
