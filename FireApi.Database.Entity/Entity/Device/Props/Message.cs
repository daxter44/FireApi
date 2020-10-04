using System;
using FireApi.Database.Entity.Entity.Device.Fields;
using FireApi.Database.Entity.Entity.Devices.Devices;
using Newtonsoft.Json;

namespace FireApi.Database.Entity.Entity.Device.Props
{
    public partial class Message
    {
        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public FieldsClass Fields { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Type { get; set; }

        [JsonProperty("lastup", NullValueHandling = NullValueHandling.Ignore)]
        public long? Lastup { get; set; }

        [JsonProperty("zz", NullValueHandling = NullValueHandling.Ignore)]
        public long? Zz { get; set; }

        [JsonProperty("passive", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Passive { get; set; }

        [JsonProperty("write", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Write { get; set; }
    }
}
