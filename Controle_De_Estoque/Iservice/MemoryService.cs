using Controle_De_Estoque.Data;
using Controle_De_Estoque.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Controle_De_Estoque.Models;
namespace Controle_De_Estoque.Iservice
{
    public class MemoryService
    {
        private readonly IMemoryCache _cache;
        private readonly IServiceAPIMercadoLivre _mercadoLivreService;
        private readonly IWooCommerceService _wooCommerceService;
        private readonly Context _context;
        public MemoryService(IMemoryCache cache, IServiceAPIMercadoLivre mercadoLivreService, IWooCommerceService wooCommerceService, Context context)
        {
            _cache = cache;
            _mercadoLivreService = mercadoLivreService;
            _wooCommerceService = wooCommerceService;
            _context = context;
        }
        public async Task<List<SerchProduct>> GetMeliAsync()
        {
            return await _cache.GetOrCreateAsync("meli_products", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                return await _mercadoLivreService.GetListProducts("226642738");
            });
        }
        public async Task<List<Products>> GetWooAsync()
        {
            return await _cache.GetOrCreateAsync("woo_products", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                return await _wooCommerceService.BuscarTodos();
            });
        }
        public async Task<List<Products>> GetWooAllListAsync(string name)
        {
            return await _cache.GetOrCreateAsync("woo_products_list", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                return await _wooCommerceService.GetListProducts(name);
            });
        }
        public async Task<List<EstoquePrincipal>> GetEstoqueAsync()
        {
            return await _cache.GetOrCreateAsync("estoque_products", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                return await _context.Estoque.ToListAsync();
            });
        }
    }
}
