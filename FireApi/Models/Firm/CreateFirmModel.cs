using FireApi.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Models.Firm
{
    public class CreateFirmModel
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public RegisterModel registerModel { get; set; }
    }
}
