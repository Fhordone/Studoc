﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studoc.Data;
using Studoc.Models;
using Microsoft.AspNetCore.Hosting;

namespace Studoc.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
    

        public ProyectoController(ILogger<HomeController> logger,
         DatabaseContext context,
         IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;
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
                if (proyecto.Imagen != null && proyecto.Imagen.Length > 0)
                {
                    // Guardar la imagen en una carpeta en el servidor
                    string rutaCarpeta = Path.Combine(_env.WebRootPath, "imagenes_proyectos");
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(proyecto.Imagen.FileName);
                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        proyecto.Imagen.CopyTo(stream);
                    }

                    proyecto.ruta_img = "/imagenes_proyectos/" + nombreArchivo;  // Guardar la ruta en el modelo
                }

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

            var proyecto = _context.Proyecto.Include(p => p.Publicacion).FirstOrDefault(p => p.ID == id);

            if (proyecto == null || proyecto.Publicacion == null)
            {
                return NotFound();
            }

            return View(proyecto.Publicacion); // Pasamos solo la publicación a la vista
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPublicacion(int id, Publicacion publicacion)
        {
            if (id != publicacion.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar la publicación directamente por su ID
                    var publicacionToUpdate = _context.Publicacion.Find(publicacion.ID);

                    if (publicacionToUpdate != null)
                    {
                        // Actualizar el contenido de la publicación
                        publicacionToUpdate.Titulo = publicacion.Titulo;
                        publicacionToUpdate.Paso_1 = publicacion.Paso_1;

                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicacionExists(publicacion.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Proyecto"); // Redirigir a la página principal de proyectos
            }

            return View(publicacion);
        }

        private bool PublicacionExists(int id)
        {
            return _context.Publicacion.Any(p => p.ID == id);
        }
    }
}
