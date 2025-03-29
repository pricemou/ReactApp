using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    [Table("Clients")] // Associe cette classe à la table "Clients"
    public class Client
    {
        [Key]
        [Column("Id_Client")]
        public int Id_Client { get; set; }

        [Required]
        [Column("Nom")]
        public string Nom { get; set; }

        [Column("Adresse")]
        public string Adresse { get; set; }

        [Column("Telephone")]
        public string Telephone { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Date_Inscription")]
        public DateTime Date_Inscription { get; set; } = DateTime.UtcNow;

        [Column("Actif")]
        public bool Actif { get; set; } = true;

        // Propriété de navigation pour la relation avec Utilisateurs via UtilisateursClients


    }
}
