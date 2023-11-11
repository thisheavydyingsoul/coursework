using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWorkAdmins.Models
{
    public class Office
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Adress { get; set; } = null!;
        public ICollection<Administrator>? Administrators { get; set; }
        public ICollection<Device>? Devices { get; set; } 
    }
}
