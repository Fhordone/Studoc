namespace Studoc.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public ICollection<UsuarioRol>? UsuariosRoles { get; set; }
    }
}
