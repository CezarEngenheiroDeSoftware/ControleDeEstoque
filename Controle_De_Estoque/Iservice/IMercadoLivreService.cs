using Controle_De_Estoque.Models;
using Microsoft.AspNetCore.Mvc;
namespace Controle_De_Estoque.Iservice
{
    public interface IMercadoLivreService
    {
        public Task<List<MercadoLivre>> GetListProducts(string id);
        public Task<SerchProduct> GetProducts(string id);
        Task<string> UpdateProductVariation(string itemId, long variationId, VariationsDTO payload);
        Task<IActionResult> VerificarToken();
    }
}
