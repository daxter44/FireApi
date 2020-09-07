using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FireApi.Database.Entity.MongoDoc
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId DocumentId { get; set; }

        System.DateTime CreatedAt { get;  }
    }
}
