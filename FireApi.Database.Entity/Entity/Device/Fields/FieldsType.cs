using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FireApi.Database.Entity.Entity.Device.Fields
{
    public partial class StringField
    {
        [BsonIgnoreIfNull]
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }
    public partial class DoubleField
    {
        [BsonIgnoreIfNull]
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public double? Double;

    }
    public partial class FromTo
    {
        [BsonIgnoreIfNull]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public Name? Name { get; set; }
        [BsonIgnoreIfNull]
        [JsonProperty("value")]
        public string? Value { get; set; }
    }
    public partial class Timer
    {
        [BsonIgnoreIfNull]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public Name? Name { get; set; }
        [BsonIgnoreIfNull]
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }

    public partial struct Value
    {
        [BsonIgnoreIfNull]
        public double? Double;

        [BsonIgnoreIfNull]
        public string String;

        public static implicit operator Value(double Double) => new Value { Double = Double };
        public static implicit operator Value(string String) => new Value { String = String };
    }
 
}
