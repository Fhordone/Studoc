using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Edit(int? id)
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
        public IActionResult Edit(int id, Proyecto proyecto)
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
            if (id == null)
            {
                return NotFound();
            }
            var create = _context.Proyecto.Find(id);
            if (create == null)
            {
                return NotFound();
            }
            return View("Publicacion");
        }

        [HttpPost]
        public IActionResult GuardarContenido(string titulo, string contenido)
        {

            var nuevoContenido = new Publicacion
            {
                Titulo = titulo,
                Contenido = contenido,

            };

            _context.Publicacion.Add(nuevoContenido);
            _context.SaveChanges(); //Falta agregarle la llave primaria del Proyecto

            return RedirectToAction("Index"); // Debe redirigir al view de la publicacion sin modificar
        }
    }
}
