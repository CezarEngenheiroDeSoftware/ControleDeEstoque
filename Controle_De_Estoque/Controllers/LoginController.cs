using Controle_De_Estoque.Data;
using Controle_De_Estoque.Models.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class LoginController : Controller
{
    private readonly ILogin _login;
    private readonly ISession _session;
    private readonly Context _context;

    public LoginController(ILogin login, ISession session, Context context)
    {
        _login = login;
        _session = session;
        _context = context;
    }

    [HttpGet]
    public IActionResult LoginExecute()
    {
        return View(new LoginExecute());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginExecute login)
    {
        var usuario = await _login.Logar(login);

        if (usuario.id != null)
        {
            return RedirectToAction("Index", "Home");
        }

        TempData["Erro"] = "E-mail ou senha inválidos";
        return RedirectToAction("LoginExecute");
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var all = await _login.GetAll();
        return View(all);
    }

    [HttpGet("ViewCreate")]
    public IActionResult ViewCreate()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Login login)
    {
        await _login.Create(login);
        return RedirectToAction("LoginExecute");
    }

    [HttpGet("ViewConfig")]
    public IActionResult ViewConfig()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Config(UserConfig config)
    {
        //var user = _session.GetSession();

       // if (user == null)
            return RedirectToAction("LoginExecute");
        //var userLogin = await _context.Logins.FirstOrDefaultAsync(a => a.Id == user.Id);
        //if(userLogin.userLogin == null)
        //{
        //    config.LoginId = userLogin.Id;

        //await _context.UserConfig.AddAsync(config);
        //await _context.SaveChangesAsync();

        //return View("ViewConfig", config);
        //}
        //else
        //{
        //    userLogin.UserConfig.MeliRefreshToken = config.MeliRefreshToken;
        //    userLogin.UserConfig.MeliClientId = config.MeliClientId;
        //    userLogin.UserConfig.MeliClientSecret = config.MeliClientSecret;
        //    userLogin.UserConfig.redirect_uri = config.redirect_uri;
        //    userLogin.UserConfig.WooUrl = config.WooUrl;
        //    userLogin.UserConfig.WooConsumerKey = config.WooConsumerKey;
        //    userLogin.UserConfig.WooConsumerSecret = config.WooConsumerSecret;

        //}
        // await _context.SaveChangesAsync();
        return View("ViewConfig", config);
    }
}
