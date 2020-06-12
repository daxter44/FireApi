using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FireApi.Database.Entity;
using MongoDB.Driver;

namespace FireApi.Database.Repository
{
    public interface IMongoDbRepository<T> where T : class
    {
        Task InsertOne(T model);
    }


    public class MongoDBGenericRepository<T> : IMongoDbRepository<T> where T : class
    {
        public IMongoDatabase Database { get; }

        public MongoDBGenericRepository(IMongoClient client)
        {
            Database = client.GetDatabase("FireDB");
        }

        public async Task InsertOne(T model)
        {
            var collectionName = GetCollectionName();
            var collection = Database.GetCollection<T>(collectionName);

            await collection.InsertOneAsync(model);
        }
        private ICollection<User> GetUser()
        {
            return Database.GetCollection<User>("users").Find<User>(user => true).ToList();
        }

        private static string GetCollectionName() { return ""; }
    }
}