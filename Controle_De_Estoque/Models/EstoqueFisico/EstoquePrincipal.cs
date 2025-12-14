using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Controle_De_Estoque.Models
{
    public class EstoquePrincipal
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage ="O nome do produto é obrigatório")]
        [MaxLength(50)]
        public string Name { get; set; } = "";
        [Required(ErrorMessage ="A quantidade de produto em estoque é obrigatório")]
        public int Quantidade_Estoque { get; set; }
        [Required(ErrorMessage ="Defina o código referente do produto")]
        [MaxLength(10)]
        public string SKU { get; set; } = "";
        [BindNever]
        public Products? produtos_WooCommerce { get; set; }
        [BindNever]
        public SerchProduct? produtos_MercadoLivre { get; set; }
    }
}
