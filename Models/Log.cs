using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWorkAdmin.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Contents { get; set; } = null!;

        [Required]
        public Administrator Administrator { get; set; } = null!;
    }
}
