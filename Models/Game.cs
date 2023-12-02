using CourseWorkAdmin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CourseWorkAdmin.Models
{
    public class Game
    {
        [Key]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public byte[]? Picture { get; set; } 
        public ICollection<DeviceGame>? DevicesGames { get; set; }

    }
}
