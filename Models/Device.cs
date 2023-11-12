using CourseWorkAdmin.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Controls;

namespace CourseWorkAdmins.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        public Office Office { get; set; } = null!;

        [MaxLength(20)]
        public string Type { get; set; } = null!;

        [MaxLength(20)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string Condition { get; set; } = null!;

        public int DayRate { get; set; }

        public int NightRate { get; set; }

        public string Description { get; set; } = null!;

        public byte[]? Picture { get; set; }
        public ICollection<DeviceGame>? DevicesGames { get; set; }
        public ICollection<Rent>? Rents { get; set; }
    }
}
