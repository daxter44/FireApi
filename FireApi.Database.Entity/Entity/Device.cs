using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Database.Entity
{
    public class Device
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public String Name { get; set; }
        public String Model { get; set; }
        public String SerialNumber { get; set; }
        public DateTime InstalationDate { get; set; }

        public DeviceStatus Status { get; set; }
        [Required]
        public virtual Client Client { get; set; }

    }
}
