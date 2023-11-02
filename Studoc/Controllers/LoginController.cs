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
                var usuario = await _context.Usuario
            .Include(u => u.UsuarioRol)
            .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync(u => u.Email == model.Email && u.Clave == model.Clave);

                if (usuario != null)
                {
                    // Autenticación exitosa
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Nombres),
                        new Claim(ClaimTypes.Surname, usuario.Apellidos),
                        new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()),
                        new Claim("Correo", usuario.Email)
                    };

                    foreach (var usuarioRol in usuario.UsuarioRol)
                    {
                        if(usuarioRol.Rol != null)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, usuarioRol.Rol.Nombre));
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

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
        public IActionResult CreateUser(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuario.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(usuario);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
