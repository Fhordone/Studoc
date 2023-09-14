using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studoc.Models
{
    public class Proyecto
    {
        [Key]
        public int ID { get; set; }
        public string? Nombre { get; set; }
        public string? D_Responsable { get; set; }
        public string? ODS { get; set; }
        public string? Semestre { get; set; }
        public string? Categoria { get; set; }
        public string? ruta_img { get; set; }
        [NotMapped]
        public bool EsIntegrante { get; set; }

        [NotMapped]
        public IFormFile? Imagen { get; set; }
        public Publicacion? Publicacion { get; set; }
        public ICollection<Rel_User_Project> Usuarios { get; set; } = new List<Rel_User_Project>();
    }
}
