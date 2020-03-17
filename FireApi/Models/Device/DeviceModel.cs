using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireApi.Entity;
using FireApi.Models.Client;

namespace FireApi.Models.Device
{
    public class DeviceModel
    {
        public Guid ID { get; set; }
        public String Name { get; set; }
        public String Model { get; set; }
        public String SerialNumber { get; set; }
        public DateTime InstalationDate { get; set; }

        public ClientModel Client { get; set; }
    }
}
