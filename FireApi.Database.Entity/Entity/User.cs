﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Database.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string EMail { get; set; }
        public string Role { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}
