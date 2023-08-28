using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Studoc.Data;
using System.Security.Claims;
using Studoc.Models;
using Microsoft.EntityFrameworkCore;

namespace Studoc.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext _context;
        //private readonly DA_Login _dalogin;
        private readonly ILogger<HomeController> _logger;
        public LoginController(ILogger<HomeController> logger,
         DatabaseContext context)
        {
            _logger = logger;
            _context = context;
           // _dalogin = dalogin;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Usuario model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == model.Email && u.Clave == model.Clave);

                if (usuario != null)
                {
                    // Autenticación exitosa
                    return RedirectToAction("Index", "Proyecto");
                }

                ModelState.AddModelError(string.Empty, "Email o clave incorrectos.");
            }

            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
