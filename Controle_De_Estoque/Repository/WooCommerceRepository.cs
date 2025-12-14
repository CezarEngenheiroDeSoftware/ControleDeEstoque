using Controle_De_Estoque.Iservice;
using Controle_De_Estoque.Models;
using Controle_De_Estoque.Models;
using Controle_De_Estoque.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Controle_De_Estoque.Repository
{
    public class WooCommerceRepository : IWooCommerceService
    {
        public readonly IHttpClientFactory _IHttpClientFactory;
        public readonly WooCommerce _woocommerce;
        public WooCommerceRepository(IHttpClientFactory httpClientFactory, WooCommerce wooCommerce)
        {
            _IHttpClientFactory = httpClientFactory;
            _woocommerce = wooCommerce;
        }
        public async Task<List<Products>> GetListProducts(string? name_Product)
        {
            var produto = await _woocommerce.ListProducts(name_Product);
            //var client = _IHttpClientFactory.CreateClient();
            //var url = $"https://localhost:7048/woocommerce/lista_products?name_Product={name_Product}";
            //var request = await client.GetAsync(url);
            //var response = await request.Content.ReadAsStringAsync();
            //var json = JsonConvert.DeserializeObject<List<Products>>(response);
            return produto;
        }
        public async Task<List<Products>> BuscarTodos()
        {
            var todos = await _woocommerce.BuscarTodos();
            //var client = _IHttpClientFactory.CreateClient();
            //var url = $"https://localhost:7048/woocommerce/BuscarTodos";
            //var request = await client.GetAsync(url);
            //var response = await request.Content.ReadAsStringAsync();
            //var json = JsonConvert.DeserializeObject<List<Products>>(response);
            return todos;
        }

        public async Task<Products> GetProducts(int id)
        {
            var product = await _woocommerce.GetProduct(id);
            //var client = _IHttpClientFactory.CreateClient();
            //var url = $"https://localhost:7048/woocommerce/GetProduct?id={id}";
            //var request = await client.GetAsync(url);
            //var response = await request.Content.ReadAsStringAsync();
            //var json = JsonConvert.DeserializeObject<Products>(response);
            return product;
        }
        public async Task<List<Products>> GetVariations(int id)
        {
            var variation = await _woocommerce.GetVariations(id);
            //var client = _IHttpClientFactory.CreateClient();
            //var url = $"https://localhost:7048/woocommerce/GetVariations?id={id}";
            //var request = await client.GetAsync(url);
            //var response = await request.Content.ReadAsStringAsync();
            //var json = JsonConvert.DeserializeObject<List<Products>>(response);
            return variation;
        }
        public async Task<List<OrdersWooCommerce>> GetAllOrders(DateTime? date_filter)
        {
            var get = await _woocommerce.GetAllOrders();
            //var client = _IHttpClientFactory.CreateClient();
            //var url = "https://localhost:7048/woocommerce/GetAllOrders";
            //var request = await client.GetAsync(url);
            ////request.Content.ReadAsStringAsync();
            //var response = await request.Content.ReadAsStringAsync();
            //var json = JsonConvert.DeserializeObject<List<OrdersWooCommerce>>(response);
            if (date_filter != null)
            {
                get = get.Where(a => a.date_created.Date == date_filter.Value.Date).ToList();
            }
            return get;
        }
        public async Task<string> UpdateProducts(int id, UpdateStock products)
        {
            var update = await _woocommerce.UpdateProductWooCommerce(id, products);
            //var client = _IHttpClientFactory.CreateClient();
            //var content = JsonConvert.SerializeObject(products);
            //var stringConent = new StringContent(content, Encoding.UTF8, "application/json");
            //var url = $"https://localhost:7048/woocommerce/UpdateProductWooCommerce?_product_id={id}";
            //var request = await client.PutAsync(url, stringConent);
            //var response = await request.Content.ReadAsStringAsync();
            return update;
        }
    }
}
