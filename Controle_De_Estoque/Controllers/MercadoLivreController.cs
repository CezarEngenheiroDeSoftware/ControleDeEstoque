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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Controle_De_Estoque.Controllers
{
    [Route("meli")]
    public class MercadoLivreController : Controller
    {
        private readonly IServiceAPIMercadoLivre _serviceAPI;
        private readonly IConfiguration _configuration;
        private readonly ISession _session;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MeliAcess _MeliAcess;
        private readonly Context _Context;
        public MercadoLivreController(IOptions<MeliAcess> MeliAcess ,IServiceAPIMercadoLivre mercadoLivreService, ISession session, IHttpClientFactory httpClientFactory, Context context, IConfiguration configuration)
        {
            _serviceAPI = mercadoLivreService;
            _session = session;
            _httpClientFactory = httpClientFactory;
            _Context = context;
            _MeliAcess = MeliAcess.Value;
            _configuration = configuration;
        }
        [HttpGet("login")]
        public async Task<IActionResult> login(int userId)
        {
            //var variable = _configuration.GetSection("client_id").Value;
            string valorDaVariavel = _configuration.GetSection("client_id").Value;

            if (valorDaVariavel == null)
            {
                Console.WriteLine("Variável não encontrada!");
            }
            var url = $"https://auth.mercadolivre.com.br/authorization?response_type=code" +
        $"&client_id={valorDaVariavel}" +
        $"&redirect_uri=https://overhonestly-unchastising-theressa.ngrok-free.dev/meli/GetAcessToken" +
        $"&state=";
            return Redirect(url);
        }


        [HttpGet("GetAcessToken")]
        public async Task<IActionResult> callback([FromQuery] string code, [FromQuery] string state)
        {
            var client = _httpClientFactory.CreateClient();
            var redirectUri = "https://controledeestoque2025-dqdbemdgagercqhw.brazilsouth-01.azurewebsites.net/meli/GetAcessToken";
            var redirectUriDesenvolvimento = "https://overhonestly-unchastising-theressa.ngrok-free.dev/meli/GetAcessToken";
            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("client_id", "715396660247853"),
            new KeyValuePair<string, string>("client_secret", "UP0pO6WTHyZUfL2LYZs5NRVBky5TWiUn"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", redirectUriDesenvolvimento)
            });

            var responseStatus = await client.PostAsync("https://api.mercadolibre.com/oauth/token", content);

            if (!responseStatus.IsSuccessStatusCode)
            {
                var erro = await responseStatus.Content.ReadAsStringAsync();
                return BadRequest(erro);
            }

            //var minhaConfig = _configuration.GetSection("MinhaChave").Value;

            var responseReady = await responseStatus.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<AcessToken>(responseReady);

            _MeliAcess.AcessToken = token.access_token;
            var tokenDb = await _Context.UserMeliToken.ToListAsync();
            if (tokenDb.Count() <= 0)
            {
                var meli_acess = new UserMeliToken
                {
                    access_token = token.access_token,
                    refreshtoken = token.refresh_token,
                    Expire_in = token.expires_in
                };
                await _Context.UserMeliToken.AddAsync(meli_acess);
            }
            else
            {
                foreach(var a in tokenDb)
                {
                    a.access_token = token.access_token;
                    a.refreshtoken = token.refresh_token;
                    a.Expire_in = token.expires_in;
                }
            }
            await _Context.SaveChangesAsync();
            TempData["logado"] = "Você está autenticado no mercado livre com sucesso";
            return RedirectToAction("Index", "Home");
        }


        [HttpGet("Index")]
        public async Task<IActionResult> Index(string? user_Id= "226642738")
        {
            //var login = _session.GetSession();
            //var user = await _Context.Logins.Include(a => a.UserMeliToken).Include(b => b.UserConfig).FirstOrDefaultAsync(x => x.Id == login.Id);
            //var userData = await _serviceAPI.GetUserData(user.UserMeliToken.access_token);
            //user_Id = userData.id;

            //var tokenVerific = await _serviceAPI.VerificarToken();

            //if (!tokenVerific)
            //{
            //    return RedirectToAction("login");
            //}

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
            //var user = _session.GetSession();
            //if (user?.UserMeliToken == null)
            //    throw new Exception("Token do Mercado Livre não encontrado na sessão.");
            //user_id = user.UserConfig.MeliClientId;
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
