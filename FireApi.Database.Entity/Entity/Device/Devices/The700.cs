using System;
using System.Collections.Generic;
using FireApi.Database.Entity.Entity.Device.Fields;
using FireApi.Database.Entity.Entity.Device.Props;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FireApi.Database.Entity.Entity.Devices.Devices
{
    public partial class The700
    {
        [JsonProperty("messages", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Message> Messages { get; set; }
    }
    

}
