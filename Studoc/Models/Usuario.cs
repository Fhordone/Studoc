using System.ComponentModel.DataAnnotations;

namespace Studoc.Models
{
    public class Usuario
    {
        [Key]
        public int ID { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
    }
}