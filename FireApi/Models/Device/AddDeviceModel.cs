using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Models.Device
{
    public class AddDeviceModel
    {
        [Required]
        public int UserId { get; set; } 
        [Required]
        public String Name { get; set; }
        public int Temperature { get; set; }


    }
}
