
namespace Controle_De_Estoque.Models
{
    public class CreateEstoquePrincipal
    {
        public EstoquePrincipal Estoque {  get; set; } = new EstoquePrincipal();
        public Products ProdutoWooCommerce { get; set; }
        public MercadoLivre ProdutoMercadoLivre {  get; set; }
    }
}
