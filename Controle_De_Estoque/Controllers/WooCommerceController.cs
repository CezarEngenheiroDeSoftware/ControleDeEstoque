using Controle_De_Estoque.Iservice;
using Controle_De_Estoque.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Controle_De_Estoque.Controllers
{
    public class WooCommerceController: Controller
    {
        
        public readonly IWooCommerceService _WooCommerceService;
        public WooCommerceController(IWooCommerceService wooCommerceService)
        {
            _WooCommerceService = wooCommerceService;
        }
        
        
        public async Task<IActionResult> Index(DateTime? date_filter, string? name, int? quantity_Product = 0)
        {
            var produto = await _WooCommerceService.BuscarTodos();
            if(quantity_Product != 0)
            {
                  produto = produto.Where(a => a.stock_quantity <= 3).Take((int)quantity_Product).ToList();
                 
            }
            //var produtofiltro = produto.Take(5);
            var orders = await _WooCommerceService.GetAllOrders(date_filter);
            var dashBoard = new DashBoardWooCommerce
            {
                Products = produto,
                orders = orders
            };
            var listaDashBoard = new List<DashBoardWooCommerce>();
            listaDashBoard.Add(dashBoard);
            return View(listaDashBoard);
        }
        public async Task<IActionResult> GetProductByName(string name)
        {
            if(name  == null)
            {
                BadRequest("O campo nome não pode ser vazio");
            }
            var product = await _WooCommerceService.GetListProducts(name);
            if(product == null)
            {
                NotFound();
            }
            var Tasks = product.Select(async p =>
            {
                p.variationsProducts = await _WooCommerceService.GetVariations(p.id);
            });
            await Task.WhenAll(Tasks);
            return View(product);
        }
        public async Task<IActionResult> Update(int id)
        {
            var produto = await FindById(id);
            var model = new UpdateStock
            {
                id = produto.id,
                sku = produto.sku,
                stock_quantity = produto.stock_quantity
            };
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProductItem([FromForm] UpdateStock product)
        {
            var producst = await _WooCommerceService.UpdateProducts(product.id, product);
            return Json(new
            {
                success=true, message = "atualizado"
            });
        }
        public async Task<UpdateStock> FindById(int id)
        {
            if(id == null || id  == 0)
            {
                return null;
            }
            var Product = await _WooCommerceService.GetProducts(id);
            var stock = new UpdateStock
            {   id = Product.id,
                sku = Product.sku,
                stock_quantity = Product.stock_quantity
            };
            return stock;
        }
        public async Task<IActionResult> GetListProducts(string? name, int? product_count = 5)
        {
            if(name == null || product_count == 0)
            {
                return NoContent();
            }
            var productsList = await _WooCommerceService.GetListProducts(name);
            if (product_count > 0)
            {
                productsList = productsList.Take((int)product_count).ToList();
            }
            foreach(var a in productsList)
            {
                if(a.variations != null)
                {
                    foreach(var b in a.variations)
                    {
                        var product = await _WooCommerceService.GetProducts(b);
                        a.variationsProducts.Add(product);
                    }
                }
            }
            return View(productsList);
        }
    }
}
