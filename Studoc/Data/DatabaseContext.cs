using Microsoft.EntityFrameworkCore;
using Studoc.Models;
using System.Collections.Generic;

namespace Studoc.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<Studoc.Models.Proyecto> Proyecto { get; set; }
        public DbSet<Studoc.Models.Publicacion> Publicacion { get; set; }
        public DbSet<Studoc.Models.Usuario> Usuario { get; set; }
        public DbSet<Studoc.Models.Rel_User_Project> Rel_User_Project  { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }
        public List<int> Usuarios { get; set; } = new List<int>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.UsuarioId, ur.RolId });

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuariosRoles)
                .HasForeignKey(ur => ur.UsuarioId);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuariosRoles)
                .HasForeignKey(ur => ur.RolId);
        }
    }
}
