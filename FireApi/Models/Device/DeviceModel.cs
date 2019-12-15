using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Models.Device
{
    public class DeviceModel
    {
        public Guid ID { get; set; }
        public String Name { get; set; }
        public int Temperature { get; set; }
    }
}
