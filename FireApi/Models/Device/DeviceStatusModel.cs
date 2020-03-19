using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Models.Device
{
    public class DeviceStatusModel
    {
        public DateTime StatusDate { get; set; }
        public decimal WaterPreasure { get; set; }
        public decimal FlowTemperature { get; set; }
        public decimal WaterTemperature { get; set; }
        public string ActiveMode { get; set; }
        public int Wetness { get; set; }
    }
}
