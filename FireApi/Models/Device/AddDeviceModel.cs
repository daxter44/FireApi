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
        public Guid Id { get; set; } 
        [Required]
        public String Name { get; set; }
        [Required]
        public String Model { get; set; }

        public DateTime InstalationDate { get; set; }
        [Required]
        public String SerialNumber { get; set; }

    }
}
