namespace Studoc.Models
{
    public class UsuarioRol
    {
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int RolId { get; set; }
        public Roles? Rol { get; set; }
    }
}
