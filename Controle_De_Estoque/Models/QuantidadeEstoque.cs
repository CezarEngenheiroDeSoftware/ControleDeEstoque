using Controle_De_Estoque.Models;
namespace Controle_De_Estoque.Models
{
    public class QuantidadeEstoque
    {
        public string id { get; set; }
        public int? available_quantity { get; set; }
        public Variations Variations { get; set; }

    }
}
