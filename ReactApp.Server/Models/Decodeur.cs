using ReactApp.Server.Data;
using System.ComponentModel.DataAnnotations;  

namespace ReactApp.Server.Models
{
        public class Decodeur
        {
            [Key]  // Indiquer que id_decodeur est la clé primaire
            public int id_decodeur { get; set; }
            public string numero_serie { get; set; }
            public string modele { get; set; }
            public int id_client { get; set; }
            public string adresse_ip { get; set; }
            public string mot_de_passe { get; set; }
            public string etat { get; set; }
            public DateTime? date_installation { get; set; }
            public DateTime? derniere_mise_a_jour { get; set; }

        // Navigation property
            public Client Client { get; set; }
        }


}
