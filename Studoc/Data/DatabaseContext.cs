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
        public DbSet<Studoc.Models.Paso> Paso { get; set; }
        public DbSet<Studoc.Models.Rel_User_Project> Rel_User_Project  { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<UsuarioRol> UsuarioRol { get; set; }
        public List<int> Usuarios { get; set; } = new List<int>();
        public List<int> Pasos { get; set; } = new List<int>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Relación de Usuario y Rol
            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.UsuarioId, ur.RolId });

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuarioRol)
                .HasForeignKey(ur => ur.UsuarioId);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuarioRol)
                .HasForeignKey(ur => ur.RolId);
            // Relación entre Proyecto y Publicacion
            modelBuilder.Entity<Proyecto>()
                .HasOne(p => p.Publicacion)
                .WithOne(p => p.Proyecto)
                .HasForeignKey<Publicacion>(p => p.ID_Proyecto);

            // Relación entre Publicacion y Pasos
            modelBuilder.Entity<Publicacion>()
                .HasMany(p => p.Pasos)
                .WithOne(p => p.Publicacion) // Aquí se especifica la propiedad de navegación inversa en Pasos
                .HasForeignKey(p => p.ID_Publicacion);
        }
    }
}
