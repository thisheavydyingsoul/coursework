using CourseWorkAdmins.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkAdmin.Models
{
    public class Rent
    {
        [Key]
        public int Id { get; set; }
        public Client Client { get; set; } = null!;
        public Device Device { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime StartDT { get; set; }
        public DateTime EndDT { get; set; }
        public Review? Review { get; set; }
    }
}
