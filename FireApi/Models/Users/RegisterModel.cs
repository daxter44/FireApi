﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Models.Users
{
    public class RegisterModel
    {
        RegisterModel()
        {
            Role = "User";
        }
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string EMail { get; set; }

        public string Role { get; set; }
    }
}
