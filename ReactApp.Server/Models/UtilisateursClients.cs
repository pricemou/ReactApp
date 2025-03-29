using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class UtilisateursClients
    {
        [Key]
        [Column("id_utilisateur")]
        public int id_utilisateur { get; set; }

        [Key]
        [Column("id_client")]
        public int id_client { get; set; }
        
        // Propriétés de navigation
    }



}