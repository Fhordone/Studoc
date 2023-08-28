using System.ComponentModel.DataAnnotations;

namespace Studoc.Models
{
    public class Proyecto
    {
        [Key]
        public int ID { get; set; }
        public string? Nombre { get; set; }
        public int D_Responsable { get; set; }
        public string? ODS { get; set; }
        public string? Semestre { get; set; }
        public string? Categoria { get; set; }
        public Publicacion? Publicacion { get; set; }
        public ICollection<Rel_User_Project> Usuarios { get; set; } = new List<Rel_User_Project>();
    }
}
