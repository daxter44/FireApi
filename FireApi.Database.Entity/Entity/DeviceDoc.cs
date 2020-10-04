using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace FireApi.Database.Entity.Entity
{
    public class DeviceDoc
    {
        public Guid DeviceID { get; set; }
        [Key]
        public String DocumentId { get; set; }
        public DateTime DocCreationTime { get; set; }
        public DeviceDoc()
        {

        }
        public DeviceDoc(Guid deviceId, String documentId, DateTime docCreationTime)
        {
            this.DeviceID = deviceId;
            this.DocumentId = documentId;
            this.DocCreationTime = docCreationTime;
        }

    }
}
