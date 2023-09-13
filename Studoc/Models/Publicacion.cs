using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studoc.Models
{
    public class Publicacion
    {
        [Key]
        public int ID { get; set; }
        public string? Titulo { get; set; }
        [ForeignKey("ID_Proyecto")]
        public int ID_Proyecto { get; set; }
        public Proyecto? Proyecto { get; set; }
        public List<Paso> Pasos { get; set; } = new List<Paso>();

    }
}