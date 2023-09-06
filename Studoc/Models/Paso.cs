using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studoc.Models
{
    public class Paso
    {
        [Key]
        public int ID { get; set; }
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public string? ruta_img { get; set; }
        [ForeignKey("ID_Publicacion")]
        public int ID_Publicacion { get; set; }
        public Publicacion? Publicacion { get; set; }
        [NotMapped]
        public byte[]? Imagen { get; set; } // Propiedad para almacenar la imagen en forma de matriz de bytes
        [NotMapped] // Esta propiedad no se mapea a la base de datos
        public IFormFile? ImagenFile { get; set; } // Propiedad para el archivo cargado de la imagen
    }
}
