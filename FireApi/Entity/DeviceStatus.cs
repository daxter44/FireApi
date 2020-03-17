using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireApi.Entity
{
    public class DeviceStatus
    {
        public Guid DeviceStatusId { get; set; }
        public Guid DeviceId { get; set; }
        public Device Device {get;set;}
        public DateTime StatusDate { get; set; }
        public decimal WaterPreasure { get; set; }
        public decimal FlowTemperature { get; set; }
        public decimal WaterTemperature { get; set; }
        public string ActiveMode { get; set; }
        public int Wetness { get; set; }
    }
}
