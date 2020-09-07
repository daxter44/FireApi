using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FireApi.Database.Entity.MongoDoc
{
    [BsonIgnoreExtraElements]
    public abstract class Document : IDocument
    {
        public ObjectId DocumentId { get; set; }

        public System.DateTime CreatedAt { get;  }
        public Document()
        {
            Random random = new Random();
            int num = random.Next(30);
            string hexString = num.ToString("X");
            DocumentId = new ObjectId(GetRandomHexNumber(32));
            CreatedAt = DateTime.Now;
        }
        static Random random = new Random();
        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }
    }
}
