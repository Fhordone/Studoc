using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studoc.Data;
using Studoc.Models;

namespace Studoc.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<HomeController> _logger;

        public ProyectoController(ILogger<HomeController> logger,
         DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            var listProject = _context.Proyecto.OrderBy(s => s.ID).ToList();
            return View("Index", listProject);
        }
        public IActionResult CrearProyecto()
        {
            ViewBag.Usuario = _context.Usuario.Select(u => new
            {
                ID = u.ID,
                NombresApellidos = $"{u.Nombres} {u.Apellidos}"
            }).ToList();
            return View("CrearProyecto"); 
        }
        public IActionResult CreateProyecto(Proyecto proyecto, List<int> UserIds)
        {
            if (ModelState.IsValid)
            {
                _context.Proyecto.Add(proyecto);
                _context.SaveChanges();

                foreach (int idUsuario in UserIds)
                {
                    var relUserProject = new Rel_User_Project
                    {
                        ID_User = idUsuario,
                        ID_Proyecto = proyecto.ID
                    };
                    _context.Rel_User_Project.Add(relUserProject);
                }
                _context.SaveChanges();

                return RedirectToAction("Index", "Proyecto");
            }

            ViewBag.Usuario = _context.Usuario.Select(u => new
            {
                ID = u.ID,
                NombresApellidos = $"{u.Nombres} {u.Apellidos}"
            }).ToList();
            return View("CrearProyecto");
        }
        public IActionResult EditProyecto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var create = _context.Proyecto.Find(id);
            if (create == null)
            {
                return NotFound();
            }
            return View(create); //Añadir Create
        }
        [HttpPost]
        public IActionResult EditProyecto(int id, Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                _context.Update(proyecto);
                _context.SaveChanges();
            }
            return View(proyecto);
        }
        public IActionResult Delete(int? id)
        {
            var create = _context.Proyecto.Find(id);
            _context.Proyecto.Remove(create); //Añadir Create
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Publicacion(int id)
        {
            var publicaciones =  _context.Proyecto.Include(u => u.Publicacion).FirstOrDefault(u => u.ID == id);
            return View(publicaciones);
        }
        public IActionResult EditPublicacion(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var publicaciones = _context.Proyecto.Include(u => u.Publicacion).FirstOrDefault(u => u.ID == id);
            if (publicaciones == null)
            {
                return NotFound();
            }
            return View("EditPublicacion");
        }

        [HttpPost]
        public IActionResult EditPublicacion(int id, Publicacion publicacion)
        {
            
                var publicaciones = _context.Proyecto.Include(u => u.Publicacion).FirstOrDefault(u => u.ID == id);
                _context.Update(publicacion);
                _context.SaveChanges();
                
            
            return View(publicaciones);
        }
    }
}
