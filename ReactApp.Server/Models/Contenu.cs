using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class Contenu
    {
        public int id_contenu { get; set; } // This corresponds to id_contenu
        public string Nom { get; set; }    // This corresponds to nom
        public string Type { get; set; }   // This corresponds to type
        public string Description { get; set; } // This corresponds to description
        public string Categorie { get; set; }  // This corresponds to categorie
        public bool Actif { get; set; }    // This corresponds to actif
    }


}