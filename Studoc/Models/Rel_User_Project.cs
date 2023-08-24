using System.ComponentModel.DataAnnotations.Schema;

namespace Studoc.Models
{
    public class Rel_User_Project
    {
        public int ID { get; set; }
        public int ID_User { get; set; }
        public int ID_Proyecto { get; set; }

        [ForeignKey("ID_User")]
        public Usuario Usuario { get; set;}
        [ForeignKey("ID_Proyecto")]
        public Proyecto Proyecto { get; set;}
    }
}
