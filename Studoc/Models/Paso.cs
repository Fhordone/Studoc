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
        public IFormFile? ImagenFile { get; set; }
    }
}
