using Controle_De_Estoque.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Controle_De_Estoque.Models.Login
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(50)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [MinLength(8)]
        [MaxLength(10)]
        public string Password { get; set; } = "";

        [Required]
        [MinLength(8)]
        [MaxLength(10)]
        [Compare(nameof(Password), ErrorMessage = "As senhas não correspondem.")]
        public string ConfirmPassword { get; set; } = "";

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [JsonIgnore]
        public UserConfig? UserConfig { get; set; } = new UserConfig();

        [JsonIgnore]
        public UserMeliToken? UserMeliToken { get; set; } = new UserMeliToken();
    }

}
