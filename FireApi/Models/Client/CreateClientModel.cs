using FireApi.Database.Entity;
using FireApi.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Models.Client
{
    public class CreateClientModel
    {
        public Guid FirmId { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        public Address Address { get; set; }
        [Required]
        public RegisterModel registerModel { get; set; }
    }
}
