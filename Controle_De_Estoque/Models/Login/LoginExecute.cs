using Controle_De_Estoque.Models.Login;
using System.ComponentModel.DataAnnotations;

namespace Controle_De_Estoque.Models.Login
{
    public class LoginExecute
    {
        [Key]
        public int id { get; set; }

        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

}

