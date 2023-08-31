using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studoc.Models
{
    public class Publicacion
    {
        [Key]
        public int ID { get; set; }
        public string? Titulo { get; set; }
        public string? Paso_1 { get; set; }
        public string? p1_img { get; set; }
        public string? Paso_2 { get; set; }
        public string? p2_img { get; set; }
        public string? Paso_3 { get; set; }
        public string? p3_img { get; set; }
        public string? Paso_4 { get; set; }
        public string? p4_img { get; set; }
        public string? Paso_5 { get; set; }
        public string? p5_img { get; set; }
        public string? Paso_6 { get; set; }
        public string? p6_img { get; set; }
        public string? Paso_7 { get; set; }
        public string? p7_img { get; set; }
        public string? Paso_8 { get; set; }
        public string? p8_img { get; set; }
        public string? Paso_9 { get; set; }
        public string? p9_img { get; set; }
        public string? Paso_10 { get; set; }
        public string? p10_img { get; set; }


        [ForeignKey("Proyecto")]
        public int ID_Proyecto { get; set; }
        public Proyecto? Proyecto { get; set; }
        
    }
}