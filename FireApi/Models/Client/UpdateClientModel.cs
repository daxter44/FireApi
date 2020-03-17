using FireApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Models.Client
{
    public class UpdateClientModel
    {

        public Guid ClientId { get; set; }
        public Guid FirmId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public User User { get; set; }
    }
}

