using System.ComponentModel.DataAnnotations;

namespace Studoc.Models
{
    public class Usuario
    {
        [Key]
        public int ID { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Email { get; set; }
        public string? Clave { get; set; }
        public DateOnly Fecha_Nacimiento { get; set; }
        public string? Genero { get; set; }
        public string? Escuela { get; set; }
        public string? Direccion { get; set; }
        public string? Codigo { get; set; }
        public ICollection<Rel_User_Project>? Proyectos { get; set; } = new List<Rel_User_Project>();
        public ICollection<UsuarioRol>? UsuarioRol { get; set; }
    }
}