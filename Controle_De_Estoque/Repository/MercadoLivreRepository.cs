using Controle_De_Estoque.Iservice;
using Controle_De_Estoque.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Controle_De_Estoque.Models;
namespace Controle_De_Estoque.Repository
{
    public class MercadoLivreRepository : IMercadoLivreService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MercadoLivreRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Autenticacao([FromQuery] string code)
        {
            var client = _httpClientFactory.CreateClient();
            var get = await client.GetAsync("https://overhonestly-unchastising-theressa.ngrok-free.dev/meli/GetAcessToken");
            get.EnsureSuccessStatusCode();
            return get.ToString();
        }
        public async Task<List<MercadoLivre>> GetListProducts(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var get = await client.GetAsync($"https://overhonestly-unchastising-theressa.ngrok-free.dev/meli/GetListProducts?userId={id}");
            Console.WriteLine(get.Content);
            get.EnsureSuccessStatusCode();
            var response = await get.Content.ReadAsStringAsync();
            Console.WriteLine("JSON LOCAL:");
            Console.WriteLine(response);

            var json = JsonConvert.DeserializeObject<List<MercadoLivre>>(response);

            return json;
        }

        public async Task<SerchProduct> GetProducts(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var get = await client.GetAsync(
                $"https://overhonestly-unchastising-theressa.ngrok-free.dev/meli/GetProduct?itemId={id}");
            get.EnsureSuccessStatusCode();
            var response = await get.Content.ReadAsStringAsync();
            Console.WriteLine(response);
            var json = JsonConvert.DeserializeObject<SerchProduct>(response);
            return json;
        }

        public async Task<string> UpdateProductVariation(string itemId, long variationId, VariationsDTO payload)
        {
            var client = _httpClientFactory.CreateClient();

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://overhonestly-unchastising-theressa.ngrok-free.dev/meli/UpdateItemVariation?iten_id={Uri.EscapeDataString(itemId)}&variationsId={variationId}";

            var response = await client.PutAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao atualizar variação (ngrok): {response.StatusCode} - {responseText}");
            }

            //var result = JsonConvert.DeserializeObject<VariationsDTO>(responseText);
            return responseText;
        }

        public async Task<IActionResult> VerificarToken()
        {
            var client = _httpClientFactory.CreateClient();
            var url = "https://overhonestly-unchastising-theressa.ngrok-free.dev/meli/VerificarToken";
            var response = await client.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao atualizar variação (ngrok): {response.StatusCode} - {responseText}");
            }
            return new JsonResult(responseText);
        }
    }
}

