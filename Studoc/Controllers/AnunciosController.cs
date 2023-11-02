using Microsoft.AspNetCore.Mvc;

namespace Studoc.Controllers
{
    public class AnunciosController : Controller
    {
        public IActionResult ViewAnuncios()
        {
            return View();
        }

        public IActionResult CreateAnuncios()
        {
            return View();
        }
        public IActionResult EditAnuncios()
        {
            return View();
        }
        public IActionResult DeleteAnuncios()
        {
            return View();
        }
    }
}
