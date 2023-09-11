namespace Studoc.Models
{
    public class UsuarioRol
    {
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }
}
