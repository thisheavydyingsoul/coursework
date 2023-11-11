using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkAdmin.Models
{
    public class Promo
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Coefficient { get; set; }
        public string Description { get; set; } = null!;
        public byte[] Picture { get; set; } = null!;
    }
}
