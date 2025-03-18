using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class JournalOperation
    {
        [Key]
        public int id_operation { get; set; }

        public int id_decodeur { get; set; }

        public int id_type_operation { get; set; }

        public int id_utilisateur { get; set; }

        public DateTime date_debut { get; set; }

        public DateTime? date_fin { get; set; }

        public string statut { get; set; }

        public string details { get; set; }
    }

}