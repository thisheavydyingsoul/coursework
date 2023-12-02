using CourseWorkAdmin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkAdmin.Models
{
    public class DeviceGame
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeviceId { get; set; }
        [Key, Column(Order = 1)]
        public string GameName { get; set; } = null!;
        public Device Device { get; set; } = null!;
        public Game Game { get; set; } = null!;
    }
}
