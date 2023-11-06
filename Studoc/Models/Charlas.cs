using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studoc.Models
{
    public class Charlas
    {
        [Key]
        public int ID { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string? Contenido { get; set; }
    }
}
