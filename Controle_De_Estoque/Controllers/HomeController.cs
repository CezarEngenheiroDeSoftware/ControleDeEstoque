using   Controle_De_Estoque.Iservice;
using Controle_De_Estoque.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Controle_De_Estoque.Iservice;

namespace Controle_De_Estoque.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISession _session;

        public HomeController(ILogger<HomeController> logger, ISession session)
        {
            _logger = logger;
            _session = session;
        }

        public IActionResult Index()
        {
            var user = _session.GetSession();

            if (user == null)
                return RedirectToAction("Login", "Login");

            return View(user);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
