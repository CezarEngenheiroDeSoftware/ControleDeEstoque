using Controle_De_Estoque.Models;
using Controle_De_Estoque.Models;

namespace Controle_De_Estoque.Iservice
{
    public interface IWooCommerceService
    {
        public Task<Products> GetProducts(int id);
        public Task<List<Products>> GetVariations(int id);
        public Task<List<Products>> GetListProducts(string? name_Product);
        public Task<List<Products>> BuscarTodos();
        public Task<string> UpdateProducts(int id, UpdateStock products);
        public Task<List<OrdersWooCommerce>> GetAllOrders(DateTime? date_filter);
    }
}
