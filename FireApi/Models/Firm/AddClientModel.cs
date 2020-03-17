using FireApi.Entity;
using FireApi.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Models.Firm
{
    public class AddClientModel
    {
        [Required]
        public Guid FirmId { get; set; } 
        [Required]
        public String FirstName { get; set; }

        public String LastName { get; set; }
        public Address Address { get; set; }

        [Required]
        public User User { get; set; }
               
        //dodac 
    }
}
