using Microsoft.EntityFrameworkCore;
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
    }
}
