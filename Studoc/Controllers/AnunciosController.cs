using Microsoft.AspNetCore.Mvc;

namespace Studoc.Controllers
{
    public class AnunciosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
