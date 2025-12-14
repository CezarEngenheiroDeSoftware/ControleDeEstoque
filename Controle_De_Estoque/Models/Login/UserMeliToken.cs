using Controle_De_Estoque.Models.Login;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Controle_De_Estoque.Models.Login
{
    public class UserMeliToken
    {
        [Key]
        public int Id { get; set; }

        public string? token { get; set; }
        public string? access_token { get; set; }
        public string? refreshtoken { get; set; }

        public int? Expire_in { get; set; }
        public DateTime datacriacao { get; set; }

        public int LoginId { get; set; }

        [JsonIgnore]
        public Login? Login { get; set; }
    }

}
