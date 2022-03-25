using Microsoft.AspNetCore.Mvc;

namespace Lanche.Controllers
{
    public class ContatoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
