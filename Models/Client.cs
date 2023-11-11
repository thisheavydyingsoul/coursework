﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkAdmin.Models
{
    public class Client
    {
        [Key]
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Fullname { get; set; } = null!;
        public int Balance { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] Picture { get; set; } = null!;
        public ICollection<Rent>? Rents {get; set;}

    }
}
