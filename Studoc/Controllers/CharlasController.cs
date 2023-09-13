using Microsoft.AspNetCore.Mvc;

namespace Studoc.Controllers
{
    public class CharlasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
