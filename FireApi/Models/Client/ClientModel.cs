using FireApi.Database.Entity;
using FireApi.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Models.Client
{
    public class ClientModel
    {

        public Guid ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public  UserModel User { get; set; }
    }
}
