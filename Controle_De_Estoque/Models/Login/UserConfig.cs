using Controle_De_Estoque.Models.Login;
using System.ComponentModel.DataAnnotations;

namespace Controle_De_Estoque.Models.Login
{
    public class UserConfig
    {
        [Key]
        public int Id { get; set; }

        public string? MeliClientId { get; set; }
        public string? MeliClientSecret { get; set; }
        public string? redirect_uri { get; set; }
        public string? MeliRefreshToken { get; set; }

        public string? WooUrl { get; set; }
        public string? WooConsumerKey { get; set; }
        public string? WooConsumerSecret { get; set; }
    }

}
