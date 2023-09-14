using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studoc.Data;
using Studoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IO;

namespace Studoc.Controllers
{
    [Authorize]
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
        public IActionResult val_integrante()
        {
            return View();
        }
        public IActionResult MyProjects()
        {
            // Obtén el usuario actual (esto puede variar según cómo gestionas la autenticación)
            var usuarioActualId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Filtra los proyectos en los que el usuario actual es un integrante
            var proyectos = _context.Proyecto
                .Where(p => p.Usuarios.Any(u => u.ID_User.ToString() == usuarioActualId))
                .ToList();

            return View("MyProjects", proyectos);
        }
        // ---------------------------------------------------FIN DE PROYECTO-----------------------------------------------------------------
        // ---------------------------------------------------iNICIO DE PUBLICACION-----------------------------------------------------------
        //Redirecciona a la visualizar Publicación
        public IActionResult Publicacion(int id)
        {
            var publicaciones = _context.Proyecto.Include(u => u.Publicacion).FirstOrDefault(u => u.ID == id);
            return View(publicaciones);
        }
        public IActionResult EditPublicacion(int id)
        {
            var publicacion = _context.Publicacion
        .Include(p => p.Pasos) // Cargar los pasos relacionados
        .FirstOrDefault(p => p.ID == id);
            if (publicacion == null)
            {
                return NotFound();
            }
            // Ahora puedes acceder a los pasos sin que se produzca una excepción de referencia nula.
            var pasos = publicacion.Pasos;

            return View(publicacion);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPublicacion(int id, Publicacion publicacion)
        {
            if (id != publicacion.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // Carga la entidad original desde la base de datos
                    var originalPublicacion = await _context.Publicacion
                        .Include(p => p.Pasos)
                        .FirstOrDefaultAsync(p => p.ID == id);
                    if (originalPublicacion == null)
                    {
                        return NotFound();
                    }
                    // Actualiza los campos de la entidad original desde la entidad modificada
                    originalPublicacion.Titulo = publicacion.Titulo;
                    // Actualiza otros campos de la publicación si es necesario

                    // Elimina los pasos que ya no existen en la entidad modificada
                    foreach (var originalPaso in originalPublicacion.Pasos.ToList())
                    {
                        if (!publicacion.Pasos.Any(p => p.ID == originalPaso.ID))
                        {
                            _context.Remove(originalPaso);
                        }
                    }
                    // Itera sobre los pasos de la publicación
                    foreach (var paso in publicacion.Pasos)
                    {
                        // Comprueba si el paso es nuevo o existente
                        if (paso.ID == 0)
                        {
                            // Este paso es nuevo, así que agrega el paso a la lista de pasos de la publicación
                            originalPublicacion.Pasos.Add(paso);
                        }
                        else
                        {
                            // Este paso ya existe, así que actualiza sus campos
                            var originalPaso = originalPublicacion.Pasos.FirstOrDefault(p => p.ID == paso.ID);
                            if (originalPaso != null)
                            {
                                originalPaso.Titulo = paso.Titulo;
                                originalPaso.Contenido = paso.Contenido;
                                // Actualiza otros campos del paso si es necesario
                            }
                        }
                    }

                    // Actualiza la entidad original en el contexto
                    _context.Update(originalPublicacion);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

            }
            return View(publicacion);
        }
        private bool PublicacionExists(int id)
        {
            return _context.Publicacion.Any(p => p.ID == id);
        }
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes_publicaciones", uniqueFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return uniqueFileName;
        }
    }
}
