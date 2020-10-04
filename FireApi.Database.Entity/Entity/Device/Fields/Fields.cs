using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FireApi.Database.Entity.Entity.Device.Fields
{
    public partial class FieldsClass
    { 
        [BsonIgnoreIfNull]
        [JsonProperty("hto", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Hto { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("hfrom", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Hfrom { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("0", NullValueHandling = NullValueHandling.Ignore)]
        public FromTo The0 { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("1", NullValueHandling = NullValueHandling.Ignore)]
        public FromTo The1 { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("2", NullValueHandling = NullValueHandling.Ignore)]
        public FromTo The2 { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("3", NullValueHandling = NullValueHandling.Ignore)]
        public FromTo The3 { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("4", NullValueHandling = NullValueHandling.Ignore)]
        public FromTo The4 { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("5", NullValueHandling = NullValueHandling.Ignore)]
        public FromTo The5 { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("tempv", NullValueHandling = NullValueHandling.Ignore)]
        public DoubleField Tempv { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("calibrationv", NullValueHandling = NullValueHandling.Ignore)]
        public DoubleField Calibrationv { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Date { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("hoursum2", NullValueHandling = NullValueHandling.Ignore)]
        public DoubleField Hoursum2 { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("offmode", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Offmode { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("mctype", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Mctype { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("rcmode", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Rcmode { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("minutes2", NullValueHandling = NullValueHandling.Ignore)]
        public DoubleField Minutes2 { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("opmode", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Opmode { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("onoff", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Onoff { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("sfmode", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Sfmode { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("shortname", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Shortname { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("yesno", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Yesno { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("mamode", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Mamode { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("zonesel", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Zonesel { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("shortphone", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Shortphone { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("energy4", NullValueHandling = NullValueHandling.Ignore)]
        public DoubleField Energy4 { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("pressv", NullValueHandling = NullValueHandling.Ignore)]
        public DoubleField Pressv { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("zname", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Zname { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("zmapping", NullValueHandling = NullValueHandling.Ignore)]
        public StringField Zmapping { get; set; }
    }


    internal class NameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Name) || t == typeof(Name?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Name.Empty;
                case "error":
                    return Name.Error;
                case "from":
                    return Name.From;
                case "to":
                    return Name.To;
            }
            throw new Exception("Cannot unmarshal type Name");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Name)untypedValue;
            switch (value)
            {
                case Name.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Name.Error:
                    serializer.Serialize(writer, "error");
                    return;
                case Name.From:
                    serializer.Serialize(writer, "from");
                    return;
                case Name.To:
                    serializer.Serialize(writer, "to");
                    return;
            }
            throw new Exception("Cannot marshal type Name");
        }

        public static readonly NameConverter Singleton = new NameConverter();
    }
}