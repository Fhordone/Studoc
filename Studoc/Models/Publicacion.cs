namespace Studoc.Models
{
    public class Publicacion
    {
        public int ID { get; set; }
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public int ID_Proyecto { get; set; }
        
    }
}