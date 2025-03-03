using ReactApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Data
{
    public class Client
    {
        [Key]
        [Column("id_client")]
        public int id_client  { get; set; }

        public string nom  { get; set; }
        public string adresse  { get; set; }
        public string telephone  { get; set; }
        public string email  { get; set; }
        public DateTime date_inscription { get; set; }
        public bool actif  { get; set; }
    }
}
