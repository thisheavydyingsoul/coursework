using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.RightsManagement;

namespace CourseWorkAdmin.Models
{
    public class Administrator
    {
        [Key, MaxLength(20)]
        public string Username { get; set; } = null!;
        public Office? Office { get; set; }

        [Required]
        [MaxLength(50)]
        public string Fullname { get; set; } = null!;


        [Required]
        public string Password { get; set; } = null!;


        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; } = null!;


        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsHr { get; set; }

        public ICollection<Log>? Logs { get; set; }
    }
}
