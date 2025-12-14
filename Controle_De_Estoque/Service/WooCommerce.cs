using Controle_De_Estoque.Iservice;
using Controle_De_Estoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Controle_De_Estoque.Service
{
    public class WooCommerce
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISession _session;
        public WooCommerce(IHttpClientFactory httpClientFactory, ISession session)
        {
            _httpClientFactory = httpClientFactory;
            _session = session;
        }
        [HttpGet("GetProduct")]
        public async Task<Products> GetProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var signature = Encoding.UTF8.GetBytes("ck_984dee04935fdbe357d30931986a8696d2e54a76:cs_602e2d15ec503f914371c9aaf6d512721dc8781f");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(signature));
            var url = $"https://lojaamplavisao.com.br/wp-json/wc/v3/products/{id}";
            var request = await client.GetAsync(url);
            var response = await request.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<Products>(response);
            return json;
        }
        [HttpGet("GetVariations")]
        public async Task<List<Products>> GetVariations(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var signature = Encoding.UTF8.GetBytes("ck_984dee04935fdbe357d30931986a8696d2e54a76:cs_602e2d15ec503f914371c9aaf6d512721dc8781f");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(signature));
            var url = $"https://lojaamplavisao.com.br/wp-json/wc/v3/products/{id}/variations";
            var request = await client.GetAsync(url);
            var response = await request.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<List<Products>>(response);
            return json;
        }
        [HttpGet("lista_products")]
        public async Task<List<Products>> ListProducts(string? name_Product)
        {
            var client = _httpClientFactory.CreateClient();
            var content = Encoding.ASCII.GetBytes("ck_984dee04935fdbe357d30931986a8696d2e54a76:cs_602e2d15ec503f914371c9aaf6d512721dc8781f");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(content));
            var responsestatus = await client.GetAsync(
                $"https://lojaamplavisao.com.br/wp-json/wc/v3/products?search={name_Product}");
            var response = await responsestatus.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<List<Products>>(response);
            //if (!string.IsNullOrEmpty(name_Product))
            //{
            //    var json_filtrador = json.Where(a => a.name.ToUpper().Contains(name_Product.ToUpper()));


            //    return Ok(json_filtrador);
            //}
            return json;
        }
        [HttpGet("BuscarTodos")]
        public async Task<List<Products>> BuscarTodos()
        {
            var client = _httpClientFactory.CreateClient();
            var content = Encoding.ASCII.GetBytes("ck_984dee04935fdbe357d30931986a8696d2e54a76:cs_602e2d15ec503f914371c9aaf6d512721dc8781f");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(content));
            var responsestatus = client.GetAsync(
                $"https://lojaamplavisao.com.br/wp-json/wc/v3/products?pag2=2&per_page=100");
            var response = await responsestatus.Result.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<List<Products>>(response);
            return json;
        }

        [HttpGet("GetAllOrders")]
        public async Task<List<OrdersWooCommerce>> GetAllOrders()
        {
            var client = _httpClientFactory.CreateClient();
            var content = Encoding.ASCII.GetBytes("ck_984dee04935fdbe357d30931986a8696d2e54a76:cs_602e2d15ec503f914371c9aaf6d512721dc8781f");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(content));
            var response = client.GetAsync($"https://lojaamplavisao.com.br/wp-json/wc/v3/orders");
            var responseStatus = await response.Result.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<List<OrdersWooCommerce>>(responseStatus);
            return json;
        }

        [HttpPut("UpdateProductWooCommerce")]
        public async Task<string> UpdateProductWooCommerce(int? _product_id, [FromBody]UpdateStock product)
        {
            var client = _httpClientFactory.CreateClient();
            var credencial = Encoding.ASCII.GetBytes("ck_984dee04935fdbe357d30931986a8696d2e54a76:cs_602e2d15ec503f914371c9aaf6d512721dc8781f");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credencial));
            var serialize_contnt = JsonConvert.SerializeObject(product);
            var content = new StringContent(serialize_contnt, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"https://lojaamplavisao.com.br/wp-json/wc/v3/products/{_product_id}", content);
            var responsestatus = response.Content.ReadAsStringAsync();
            return responsestatus.ToString();
        }
    }
}
