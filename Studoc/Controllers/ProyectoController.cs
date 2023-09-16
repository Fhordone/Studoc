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
            // Obtener la publicación deseada por su ID
            var publicacion = _context.Publicacion
                .Include(p => p.Pasos) // Incluye los pasos relacionados si es necesario
                .FirstOrDefault(p => p.ID == id);

            if (publicacion == null)
            {
                return NotFound(); // Manejo de caso en el que la publicación no se encuentra
            }

            return View(publicacion); // Pasar la instancia de Publicacion como modelo
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
                    var originalPublicacion = await _context.Publicacion
                        .Include(p => p.Pasos)
                        .FirstOrDefaultAsync(p => p.ID == id);

                    if (originalPublicacion == null)
                    {
                        return NotFound();
                    }
                    // Actualiza los campos de la entidad original desde la entidad modificada
                    originalPublicacion.Titulo = publicacion.Titulo;
                    foreach (var originalPaso in originalPublicacion.Pasos.ToList())
                    {
                        if (!publicacion.Pasos.Any(p => p.ID == originalPaso.ID))
                        {
                            _context.Remove(originalPaso);
                        }
                    }
                    foreach (var paso in publicacion.Pasos)
                    {
                        if (paso.ID == 0)
                        {
                            // Paso nuevo
                            if (paso.ImagenFile != null)
                            {
                                // Obtén la ruta raíz de wwwroot
                                var webRootPath = _env.WebRootPath;

                                // Construye la ruta de la imagen dentro de wwwroot/imagenes_publicaciones
                                var imagePath = Path.Combine(webRootPath, "imagenes_publicaciones", paso.ImagenFile.FileName);

                                // Procesa y almacena la imagen
                                using (var stream = new FileStream(imagePath, FileMode.Create))
                                {
                                    await paso.ImagenFile.CopyToAsync(stream);
                                }

                                paso.ruta_img = Path.Combine("imagenes_publicaciones", paso.ImagenFile.FileName); // Actualiza la propiedad RutaImagen
                            }

                            originalPublicacion.Pasos.Add(paso);
                        }
                        else
                        {
                            // Paso existente
                            var originalPaso = originalPublicacion.Pasos.FirstOrDefault(p => p.ID == paso.ID);
                            if (originalPaso != null)
                            {
                                originalPaso.Titulo = paso.Titulo;
                                originalPaso.Contenido = paso.Contenido;

                                if (paso.ImagenFile != null)
                                {
                                    // Obtén la ruta raíz de wwwroot
                                    var webRootPath = _env.WebRootPath;

                                    // Construye la ruta de la imagen dentro de wwwroot/imagenes_publicaciones
                                    var imagePath = Path.Combine(webRootPath, "imagenes_publicaciones", paso.ImagenFile.FileName);

                                    // Procesa y almacena la imagen
                                    using (var stream = new FileStream(imagePath, FileMode.Create))
                                    {
                                        await paso.ImagenFile.CopyToAsync(stream);
                                    }

                                    originalPaso.ruta_img = Path.Combine("imagenes_publicaciones", paso.ImagenFile.FileName); // Actualiza la propiedad RutaImagen
                                }
                                
                            }
                        }
                    }

                    _context.Update(originalPublicacion);
                    await _context.SaveChangesAsync();
                    // Recarga la publicación desde la base de datos después de guardar los cambios
                    originalPublicacion = await _context.Publicacion
                        .Include(p => p.Pasos)
                        .FirstOrDefaultAsync(p => p.ID == id);

                    return RedirectToAction("Publicacion", new { id = originalPublicacion.ID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return View(publicacion);
        }
    }
}
