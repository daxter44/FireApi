﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Database.Entity
{
    public class Firm 
    {

        [ForeignKey(nameof(User))]
        public Guid FirmId { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public User User { get; set; }
        public ICollection<Client> Clients { get; set; }
    }
}
