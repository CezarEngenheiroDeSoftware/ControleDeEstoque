using Controle_De_Estoque.Data;
using Controle_De_Estoque.Iservice;
using Controle_De_Estoque.Models.Login;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class SessionRepository : ISession
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly Context _context;

    public SessionRepository(IHttpContextAccessor httpContextAccessor, Context context)
    {
        _contextAccessor = httpContextAccessor;
        _context = context;
    }

    public void CreateSession(Login login)
    {
        var session = new UserSession
        {
            Id = login.Id,
            Email = login.Email
        };
        var json = JsonConvert.SerializeObject(session);
        _contextAccessor.HttpContext.Session.SetString("usuario", json);
    }

    public void CreateSessionToMeli(Login login)
    {
        var json = JsonConvert.SerializeObject(login);
        _contextAccessor.HttpContext.Session.SetString("usuario_meli", json);
    }
    public Login? GetSessionToMeli()
    {
        var sessao = _contextAccessor.HttpContext.Session.GetString("usuario_meli");

        if (sessao != null)
            return JsonConvert.DeserializeObject<Login>(sessao);

        return null;
    }
    public Login? GetSession()
    {
        var sessao = _contextAccessor.HttpContext.Session.GetString("usuario");
        
        if (sessao != null)
            return JsonConvert.DeserializeObject<Login>(sessao);

        return null;
    }

    public async Task<Login?> LoginAsync(LoginExecute login)
    {
        return await _context.Logins
            .Include(x => x.UserConfig)
            .Include(x => x.UserMeliToken)
            .FirstOrDefaultAsync(a => a.Email == login.Email && a.Password == login.Password);
    }

    public void RemoveSession(Login login)
    {
        _contextAccessor.HttpContext.Session.Remove("usuario");
    }
}
