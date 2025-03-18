using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class TypesOperation
    {
        [Key]
        public int id_type_operation { get; set; }

        public string nom { get; set; }

        public string description { get; set; }

        public int temps_execution_moyen { get; set; } 
    }


}