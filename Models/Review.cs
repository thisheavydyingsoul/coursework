using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkAdmin.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public Rent Rent { get; set; } = null!;
        public string Contents { get; set; } = null!;
        public int Rating { get; set; }
    }
}
