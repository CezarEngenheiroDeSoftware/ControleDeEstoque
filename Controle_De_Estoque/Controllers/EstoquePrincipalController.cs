
using Controle_De_Estoque.Iservice;
using Controle_De_Estoque.Models;
using Controle_De_Estoque.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Controle_De_Estoque.Controllers
{
    public class EstoquePrincipalController : Controller
    {
        private readonly Data.Context _context;
        private readonly IServiceAPIMercadoLivre _mercadoLivreService;
        private readonly IWooCommerceService _woocommerceService;
        private readonly MemoryService _memoryService;
        public EstoquePrincipalController(MemoryService memoryService ,Data.Context context, IWooCommerceService woocommerceService, IServiceAPIMercadoLivre mercadoLivreService)
        {
            _context = context;
            _woocommerceService = woocommerceService;
            _mercadoLivreService = mercadoLivreService;
            _memoryService = memoryService;
        }
        public async Task<IActionResult> Index(string? nome)
        {
            //var ProdutosWitchName = await _context.Estoque.Where(a => a.Name.Contains(nome)).ToListAsync();
            //var TodosProdutos = await _context.Estoque.ToListAsync();
            //var meli_ProdWitchName = await _mercadoLivreService.GetListProducts("226642738");
            //var woo_ProdWitchName = await _woocommerceService.BuscarTodos();
            //var produtoMercadoLivre = HttpContext.Items["meli_ProdWitchName"] as List<MercadoLivre> ?? await _mercadoLivreService.GetListProducts("226642738");
            //HttpContext.Items["meli_ProdWitchName"] = produtoMercadoLivre;
            //var produtoWooCommerce = HttpContext.Items["woo_ProdWitchName"] as List<Products> ?? await _woocommerceService.BuscarTodos();
            //HttpContext.Items["woo_ProdWitchName"] = produtoWooCommerce;
            //var produtoFisico = HttpContext.Items["ProdutosWitchName"] as List<EstoquePrincipal> ?? await _context.Estoque.Where(a => a.Name.Contains(nome)).ToListAsync();
            //HttpContext.Items["ProdutosWitchName"] = produtoFisico;
            //var produto = HttpContext.Items["TodosProdutos"] as List<EstoquePrincipal> ?? await _context.Estoque.ToListAsync();
            //HttpContext.Items["ProdutosWitchName"] = produto;

            var produtoFisico = await _memoryService.GetEstoqueAsync();
            var produtoMercadoLivre = await _memoryService.GetMeliAsync();
            var produtoWooCommerce = await _memoryService.GetWooAsync();

            if (!string.IsNullOrEmpty(nome))
            {
                var produtoFisicoWithName = produtoFisico.Where(a=>a.Name.Contains(nome)).ToList();
                var contentWithName = produtoFisicoWithName.Select(a => new EstoquePrincipal
                {
                    Id = a.Id,
                    Name = a.Name,
                    Quantidade_Estoque = a.Quantidade_Estoque,
                    SKU = a.SKU,
                    produtos_WooCommerce = produtoWooCommerce.FirstOrDefault(b => b.sku == a.SKU),
                    produtos_MercadoLivre = produtoMercadoLivre.FirstOrDefault(c => c.variations.Any(v => v.seller_custom_field == a.SKU))
                }).ToList();
                return View(contentWithName);
            }
                var content = produtoFisico.Select(a => new EstoquePrincipal
                {
                Id = a.Id,
                Name = a.Name,
                Quantidade_Estoque = a.Quantidade_Estoque,
                SKU = a.SKU,
                produtos_WooCommerce = produtoWooCommerce.FirstOrDefault(b =>
                b.sku == a.SKU),
                
                produtos_MercadoLivre = produtoMercadoLivre.FirstOrDefault(c => c.variations.Any(v => v.seller_custom_field == a.SKU))
            }).ToList();
            return View(content);
        }
        public async Task<IActionResult> GetProductById(Guid id)
        {
            if(id != Guid.Empty)
            {
                var Produto = await _context.Estoque.FirstOrDefaultAsync(x => x.Id == id);
                if(Produto == null)
                {
                    return NoContent();
                }
                else
                {
                    return View(Produto);
                }
            }
            ViewData["id"] = $"O id deve ser passado para busca";
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> CreateView()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EstoquePrincipal estoquePrincipal)
        {
            if(ModelState.IsValid)
            {
                estoquePrincipal.Id = Guid.NewGuid();
                var novo_produto = await _context.AddAsync(estoquePrincipal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            Console.WriteLine(ModelState);
            return BadRequest(Problem);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            if(id  == Guid.Empty)
            {
                return BadRequest(Problem("o id não pode ficar vazio"));
            }
            var produto = await _context.Estoque.FirstOrDefaultAsync(a=>a.Id == id);
            return PartialView(produto);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEstoque([FromForm] EstoquePrincipal principal)
        {
            var produto = await _context.Estoque.FirstOrDefaultAsync(_ => _.Id == principal.Id);
            produto.SKU = principal.SKU;
            produto.Name = principal.Name;
            produto.Quantidade_Estoque = principal.Quantidade_Estoque;
            produto.produtos_MercadoLivre = principal.produtos_MercadoLivre;
            produto.produtos_WooCommerce = principal.produtos_WooCommerce;
            _context.Update(produto);
            await _context.SaveChangesAsync();
            var tarefasMeli = new List<Task>();
            var tarefasWoo = new List<Task>();
            var meli = await _memoryService.GetMeliAsync();
            var meli_prod = meli.FirstOrDefault(a => a.variations.Any(b => b.seller_custom_field == produto.SKU));
            if (meli_prod == null)
            {
                TempData["SincronizarFaill"] = "Não foi possível sincronizar todos os produtos. É necessário a referência do" +
                        "Mercado Live e da Loja Virtual";
                return Json(new{ success = false });
            }
            var itenId = meli_prod.id;
            if (meli_prod != null)
            {
                foreach(var a in meli_prod.variations)
                {
                    var payload = new VariationsDTO
                    {
                        id = a.variationsId,
                        seller_custom_field = produto.SKU,
                        available_quantity = produto.Quantidade_Estoque,
                    };
                    tarefasMeli.Add(_mercadoLivreService.UpdateItemVariation(itenId, payload, a.variationsId));
                }
            }

                var woo = await _memoryService.GetWooAsync();
                var wooIten = woo.FirstOrDefault(p=>p.sku == produto.SKU);
            if (wooIten == null)
            {
                TempData["SincronizarFaill"] = "Não foi possível sincronizar todos os produtos. É necessário a referência do " +
                        "Mercado Livre e da Loja Virtual";
                return Json(new {success = false });
            }

            if (wooIten != null)
            {
                var updateStock = new UpdateStock
                {
                    id = wooIten.id,
                    sku = produto.SKU,
                    stock_quantity = produto.Quantidade_Estoque
                };
                if(updateStock.stock_quantity == null||updateStock.stock_quantity < 0)
                {
                    return Json(new { success = false, message = "A quantidade de estoque não pode ser vazia" });
                    
                }
                tarefasWoo.Add(_woocommerceService.UpdateProducts(wooIten.id, updateStock));
            }
                await Task.WhenAll(tarefasMeli.Concat(tarefasWoo));
                return Json(new { success = true});
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await _context.Estoque.FindAsync(id);
            
            return View(produto);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var produto = await _context.Estoque.FindAsync(id);

            if (produto == null)
                return NotFound();

            _context.Estoque.Remove(produto);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Sincronizar()
        {
            var produtoEstoqueFisico = await _memoryService.GetEstoqueAsync();
            var produtosMercadoLivre = await _memoryService.GetMeliAsync();
            var produtosWooCommerce = await _memoryService.GetWooAsync();
            var produto = produtoEstoqueFisico.Select(a => new EstoquePrincipal
            {
                Name = a.Name,
                SKU = a.SKU,
                Id = a.Id,
                Quantidade_Estoque = a.Quantidade_Estoque,
                produtos_MercadoLivre = produtosMercadoLivre.FirstOrDefault(C => C.variations.Any(b => b.seller_custom_field == a.SKU)),
                produtos_WooCommerce = produtosWooCommerce.FirstOrDefault(x => x.sku == a.SKU)
            }).ToList();
            var tarefasMeli = new List<Task>();
            var tarefasWoo = new List<Task>();
            foreach(var p in produto)
            {
                if(p.produtos_MercadoLivre == null || p.produtos_WooCommerce == null)
                {
                    TempData["SincronizarFaill"] = "Não foi possível sincronizar todos os produtos. É necessário a referência do " +
                        "Mercado Livre e da Loja Virtual";
                    return RedirectToAction("Index");
                }
                if(p.produtos_MercadoLivre != null)
                {
                    var ml = p.produtos_MercadoLivre;
                    if (ml.available_quantity != p.Quantidade_Estoque)
                    {
                        foreach (var v in ml.variations)
                        {
                            var payload = new VariationsDTO
                            {
                                seller_custom_field = v.seller_custom_field,
                                available_quantity = p.Quantidade_Estoque,
                                id = v.variationsId,
                            };
                            tarefasMeli.Add(_mercadoLivreService.UpdateItemVariation(ml.id, payload, v.variationsId));
                            
                        }
                    }
                }
                if(p.produtos_WooCommerce != null)
                {
                    var woo = p.produtos_WooCommerce;
                    if (woo.stock_quantity != p.Quantidade_Estoque)
                    {
                        var payload = new UpdateStock
                        {
                            id = woo.id,
                            stock_quantity = p.Quantidade_Estoque,
                            sku = p.SKU
                        };
                        tarefasWoo.Add(_woocommerceService.UpdateProducts(woo.id, payload));
                    }
                }
                
            }
            await Task.WhenAll(tarefasMeli.Concat(tarefasWoo));
            TempData["Check"] = "atualizado..";
            return RedirectToAction("Index");
        }
    }
}
