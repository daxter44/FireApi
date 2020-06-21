using System;
using MongoDB.Bson;

namespace FireApi.Database.Entity.MongoDoc
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public System.DateTime CreatedAt => Id.CreationTime;
    }
}
