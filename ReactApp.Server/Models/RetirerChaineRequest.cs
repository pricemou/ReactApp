using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    
    public class RetirerChaineRequest
    {
        public int id_decodeur { get; set; }
        public int id_contenu { get; set; }
    }

}