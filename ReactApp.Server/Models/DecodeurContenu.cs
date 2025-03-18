using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class DecodeurContenu
        {
            public int id_decodeur { get; set; }  // Clé étrangère vers la table Decodeurs
            public int id_contenu { get; set; }   // Clé étrangère vers la table Contenus

            // Navigation properties (facultatif, pour les relations avec les autres entités)
            public Decodeur Decodeur { get; set; }  // Relation avec la table Decodeurs
            public Contenu Contenu { get; set; }    // Relation avec la table Contenus
        }


}