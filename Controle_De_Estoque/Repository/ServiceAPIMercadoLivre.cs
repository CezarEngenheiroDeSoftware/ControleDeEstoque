using Controle_De_Estoque.Data;
using Controle_De_Estoque.Model;
using Controle_De_Estoque.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

public class ServiceAPIMercadoLivre : IServiceAPIMercadoLivre
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Context _Context;
    private readonly ISession _session;
    private readonly MeliAcess _MeliAcess;
    private readonly IConfiguration _Configuration;

    public ServiceAPIMercadoLivre(
        IHttpClientFactory httpClientFactory,
        Context context,
        IOptions<MeliAcess> MeliAcess,
        ISession session,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _Context = context;
        _session = session;
        _MeliAcess = MeliAcess.Value;
        _Configuration = configuration;
    }

    //public async Task<bool> VerificarToken()
    //{
        //var sessionUser = _session.GetSession();

        //if (sessionUser == null)
        //    return false;

        //var user = await _Context.Logins
        //    .Include(x => x.UserMeliToken)
        //    .Include(x => x.UserConfig)
        //    .FirstOrDefaultAsync(x => x.Id == sessionUser.Id);
        //if(user ==  null) return false;
        //var token = user.UserMeliToken;

        //if (token == null)
        //    return false;
        //if(!token.Expire_in.HasValue) return false;
        //DateTime expira = token.datacriacao.AddSeconds((double)token.Expire_in);

        //if (DateTime.Now < expira)
        //    return true;

        //if (token.refreshtoken == null)
        //    return false;

        //var novo = await RefreshToken(token.refreshtoken);

        //token.access_token = novo.access_token;
        //token.refreshtoken = novo.refresh_token;
        //token.Expire_in = novo.expires_in;
        //token.datacriacao = DateTime.Now;

        //await _Context.SaveChangesAsync();
        //_session.CreateSession(user);

        //return true;
    //}

    public async Task<UserData> GetUserData(string token)
    {
        //var user = _session.GetSession();
        //var userlogin = await _Context.Logins.Include(a => a.UserConfig).Include(b => b.UserMeliToken).FirstOrDefaultAsync(x => x.Id == user.Id);
        //if (userlogin.UserMeliToken.access_token == null)
        //    throw new Exception("Token não encontrado na sessão.");

        //await VerificarToken();
        var acessdb = await _Context.UserMeliToken.ToListAsync();
        var acessToken = "";
        foreach (var a in acessdb)
        {
            acessToken = a.access_token;
        }

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", acessToken);

        var response = await client.GetStringAsync("https://api.mercadolibre.com/users/me");

        return JsonConvert.DeserializeObject<UserData>(response);
    }

    public async Task<SerchProduct> GetProduct(string itemId)
    {
        //var user = _session.GetSession();
        //var usertoken = await _Context.Logins.
        //    Include(a => a.UserMeliToken).
        //    Include(b=>b.UserConfig).FirstOrDefaultAsync((x=>x.Id == user.Id));
        //if (usertoken?.UserMeliToken == null)
        //    throw new Exception("Token não encontrado.");

        //await VerificarToken();
        var acessdb = await _Context.UserMeliToken.ToListAsync();
        var acessToken = "";
        foreach(var a in acessdb)
        {
            acessToken = a.access_token;
        }
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", acessToken);

        var response = await client.GetStringAsync($"https://api.mercadolibre.com/items/{itemId}");

        return JsonConvert.DeserializeObject<SerchProduct>(response);
    }

    public async Task<List<SerchProduct>> GetListProducts(string userId)
    {
        //var user = _session.GetSession();
        //var user_token = await _Context.Logins.
        //    Include(a => a.UserMeliToken).
        //    Include(b => b.UserConfig).FirstOrDefaultAsync(a=>a.Id == user.Id);
        //if (user_token == null)
        //    throw new Exception("Token inválido.");

        //await VerificarToken();
        var acessdb = await _Context.UserMeliToken.ToListAsync();
        var acessToken = "";
        foreach (var a in acessdb)
        {
            acessToken = a.access_token;
        }
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", acessToken);

        var jsonString = await client.GetStringAsync(
            $"https://api.mercadolibre.com/users/{userId}/items/search");

        var items = JsonConvert.DeserializeObject<Controle_De_Estoque.Model.Items_Id>(jsonString);

        var list = new List<SerchProduct>();

        foreach (var id in items.results)
        {
            list.Add(await GetProduct(id));
        }

        return list;
    }

    public async Task<VariationsDTO> UpdateItemVariation(string itemId, VariationsDTO variations, long variationId)
    {
        //var user = _session.GetSession();
        //var user_token = await _Context.Logins.
        //    Include(a => a.UserMeliToken).
        //    Include(b => b.UserConfig).
        //    FirstOrDefaultAsync(x => x.Id == user.Id);
        //if (user_token?.UserMeliToken == null)
        //    throw new Exception("Sessão expirada.");

        //await VerificarToken();
        var acessdb = await _Context.UserMeliToken.ToListAsync();
        var acessToken = "";
        foreach (var a in acessdb)
        {
            acessToken = a.access_token;
        }
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", acessToken);

        var json = JsonConvert.SerializeObject(variations);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync(
            $"https://api.mercadolibre.com/items/{itemId}/variations/{variationId}",
            content);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(responseContent);

        return JsonConvert.DeserializeObject<VariationsDTO>(responseContent);
    }

    public async Task<AcessToken> RefreshToken(string refreshToken)
    {
        //var user = _session.GetSession();

        //var user_Login = await _Context.Logins
        //    .Include(a => a.UserConfig)
        //    .FirstOrDefaultAsync(a => a.Id == user.Id);

        var client = _httpClientFactory.CreateClient();
        var variable_ambient_ClientId = _Configuration.GetSection("client_id").Value;
        var variable_ambient_ClienteSecret = _Configuration.GetSection("client_secret").Value; 
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type","refresh_token"),
            new KeyValuePair<string, string>("client_id", variable_ambient_ClientId),
            new KeyValuePair<string, string>("client_secret", variable_ambient_ClienteSecret),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        });

        var response = await client.PostAsync("https://api.mercadolibre.com/oauth/token", content);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(json);

        return JsonConvert.DeserializeObject<AcessToken>(json);
    }
    public async Task<string> SearchAttribute(string categ)
    {
        //var user = _session.GetSession();
        //if (user?.UserMeliToken == null)
        //    throw new Exception("Token inválido.");

        //await VerificarToken();
        var acessdb = await _Context.UserMeliToken.ToListAsync();
        var acessToken = "";
        foreach (var a in acessdb)
        {
            acessToken = a.access_token;
        }
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", acessToken);

        return await client.GetStringAsync($"https://api.mercadolibre.com/categories/{categ}/attributes");
    }

    public async Task<string> SearchCategory(string q)
    {
        //var user = _session.GetSession();
        //if (user?.UserMeliToken == null)
        //    throw new Exception("Token inválido.");

        //await VerificarToken();
        var acessdb = await _Context.UserMeliToken.ToListAsync();
        var acessToken = "";
        foreach (var a in acessdb)
        {
            acessToken = a.access_token;
        }
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", acessToken);

        return await client.GetStringAsync($"https://api.mercadolibre.com/sites/MLB/domain_discovery/search?q={q}");
    }

    public async Task<PostParaAnuncio> CreateItem(PostParaAnuncio item)
    {
        //var user = _session.GetSession();
        //if (user?.UserMeliToken == null)
        //    throw new Exception("Token inválido.");

        //await VerificarToken();
        var acessdb = await _Context.UserMeliToken.ToListAsync();
        var acessToken = "";
        foreach (var a in acessdb)
        {
            acessToken = a.access_token;
        }
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", acessToken);

        var json = JsonConvert.SerializeObject(item);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://api.mercadolibre.com/items", content);
        var resultJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(resultJson);

        return JsonConvert.DeserializeObject<PostParaAnuncio>(resultJson);
    }

    public async Task<ItemMeli> UpdateItem(string itemId, ItemMeli updateData)
    {
        //var user = _session.GetSession();
        //if (user?.UserMeliToken == null)
        //    throw new Exception("Token inválido.");

        //await VerificarToken();
        var acessdb = await _Context.UserMeliToken.ToListAsync();
        var acessToken = "";
        foreach (var a in acessdb)
        {
            acessToken = a.access_token;
        }
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", acessToken);

        var json = JsonConvert.SerializeObject(updateData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"https://api.mercadolibre.com/items/{itemId}", content);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(responseJson);

        return JsonConvert.DeserializeObject<ItemMeli>(responseJson);
    }

}
