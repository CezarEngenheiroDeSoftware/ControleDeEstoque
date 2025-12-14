using Controle_De_Estoque.Data;
using Controle_De_Estoque.Iservice;
using Controle_De_Estoque.Model;
using Controle_De_Estoque.Models;
using Controle_De_Estoque.Models;
using Controle_De_Estoque.Models.Login;
using Controle_De_Estoque.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;

namespace Controle_De_Estoque.Controllers
{
    [Route("meli")]
    public class MercadoLivreController : Controller
    {
        private readonly IServiceAPIMercadoLivre _serviceAPI;
        private readonly ISession _session;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Context _Context;
        public MercadoLivreController(IServiceAPIMercadoLivre mercadoLivreService, ISession session, IHttpClientFactory httpClientFactory, Context context)
        {
            _serviceAPI = mercadoLivreService;
            _session = session;
            _httpClientFactory = httpClientFactory;
            _Context = context;
        }
        [HttpGet("login")]
        public IActionResult login(int userId)
        {
            var url = $"https://auth.mercadolivre.com.br/authorization?response_type=code" +
        $"&client_id=4545555147210680" +
        $"&redirect_uri=https://overhonestly-unchastising-theressa.ngrok-free.dev/meli/GetAcessToken" +
        $"&state={userId}";
            return Redirect(url);
        }


        [HttpGet("GetAcessToken")]
        public async Task<IActionResult> callback([FromQuery] string code, [FromQuery] string state)
        {
            int userId = int.Parse(state);

            var user_Login = await _Context.Logins
                .Include(a => a.UserConfig)
                .Include(a => a.UserMeliToken)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user_Login == null)
                return BadRequest("Usuário não encontrado.");

            var client = _httpClientFactory.CreateClient();

            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("grant_type", "authorization_code"),
        new KeyValuePair<string, string>("client_id", user_Login.UserConfig.MeliClientId),
        new KeyValuePair<string, string>("client_secret", user_Login.UserConfig.MeliClientSecret),
        new KeyValuePair<string, string>("code", code),
        new KeyValuePair<string, string>("redirect_uri", user_Login.UserConfig.redirect_uri)
    });

            var responseStatus = await client.PostAsync("https://api.mercadolibre.com/oauth/token", content);

            if (!responseStatus.IsSuccessStatusCode)
                return BadRequest("Erro ao obter token.");

            var responseReady = await responseStatus.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<AcessToken>(responseReady);

            if (user_Login.UserMeliToken == null)
                user_Login.UserMeliToken = new UserMeliToken();

            user_Login.UserMeliToken.access_token = token.access_token;
            user_Login.UserMeliToken.refreshtoken = token.refresh_token;
            user_Login.UserMeliToken.Expire_in = token.expires_in;
            user_Login.UserMeliToken.datacriacao = DateTime.Now;
            user_Login.UserMeliToken.LoginId = userId;

            await _Context.SaveChangesAsync();

            var userAtualizado = await _Context.Logins
            //    .Include(x => x.UserConfig)
            //    .Include(x => x.UserMeliToken)
            .FirstOrDefaultAsync(x => x.Id == userId);

            _session.CreateSession(userAtualizado);
            TempData["logado"] = "Autenticado com sucesso";
            return RedirectToAction("Index", "Home");
        }


        [HttpGet("Index")]
        public async Task<IActionResult> Index(string? user_Id = "226642738")
        {
            //var user = _session.GetSession();

            //if (user == null)
            //{
            //    return RedirectToAction("login");
            //}

            var tokenVerific = await _serviceAPI.VerificarToken();

            if (!tokenVerific)
            {
                return RedirectToAction("login");
            }

            var products = await _serviceAPI.GetListProducts(user_Id);

            return View(products);
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts(string id)
        {
            //id = "MLB5865918002";
            if (id != null)
            {
                var products = await _serviceAPI.GetProduct(id);
                return View(products);
            }
            return BadRequest();
        }
        [HttpGet("GetListProduct")]
        public async Task<IActionResult> GetListProduct(string? user_id)
        {
            var user = _session.GetSession();
            if (user?.UserMeliToken == null)
                throw new Exception("Token do Mercado Livre não encontrado na sessão.");
            user_id = user.UserConfig.MeliClientId;
            if (user_id != null)
            {
                var list_products = await _serviceAPI.GetListProducts(user_id);
                
                return View(list_products);
            }
            return BadRequest();
        }
        [HttpGet("Update")]
        public async Task<IActionResult> Update(string id)
        {
            if(id != null)
            {
                var product = await _serviceAPI.GetProduct(id);
                return View(product);
            }
            return BadRequest();
        }
        [HttpPost("UpdateItem")]
        public async Task<IActionResult> UpdateItem(string id, long variationId, int available_quantity, string seller_custom_field)
        {
            if (string.IsNullOrEmpty(id) || variationId == 0)
                return BadRequest("Item id e variationId são obrigatórios.");
            var payload = new VariationsDTO
            {
                id = variationId,              
                available_quantity = available_quantity,
                seller_custom_field = seller_custom_field
            };
            try
            {
                var updated = await _serviceAPI.UpdateItemVariation(id, payload, variationId);
                TempData["Success"] = "Estoque atualizado com sucesso.";
                return RedirectToAction("GetProducts", new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro UpdateItem: {ex}");
                TempData["Error"] = ex.Message;
                return RedirectToAction("Update", new { id = id });
            }
        }


    }
}
