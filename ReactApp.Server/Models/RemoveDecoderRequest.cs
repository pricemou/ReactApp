using System.Text.Json.Serialization;

namespace ReactApp.Server.Models
{
    // Représente les données envoyées dans la requête pour retirer un décodeur d'un client
    public class RemoveDecoderRequest
    {
        // L'ID du client
        public int id_client { get; set; }

        // L'ID du décodeur à retirer
        public int id_decodeur { get; set; }
    }

}
