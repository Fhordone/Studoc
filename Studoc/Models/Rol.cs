namespace Studoc.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public ICollection<UsuarioRol>? UsuarioRol { get; set; }
    }
}
