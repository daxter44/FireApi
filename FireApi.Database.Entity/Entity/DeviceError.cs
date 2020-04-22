using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireApi.Database.Entity
{
    public class DeviceError
    {
        [Key]
        Guid DeviceErrorId { get; set; }
        public Guid DeviceStatusId { get; set; }
        [ForeignKey(nameof(DeviceStatusId))]
        public virtual DeviceStatus DeviceStatus { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }
}
