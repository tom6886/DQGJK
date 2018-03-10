using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;

namespace DQGJK.Service
{
    internal class MongoHandler
    {
        private static string _MongoDbConnectionStr = ConfigurationManager.AppSettings["mongodb"];

        internal static void Save<T>(T t)
        {
            GetCollection<T>().InsertOne(t);
        }

        internal static void SaveOfBson<T>(T t)
        {
            BsonDocument bd = t.ToBsonDocument();
            GetBsonCollection<T>().InsertOne(bd);
        }

        internal static IMongoCollection<T> GetCollection<T>(string collectionName = null)
        {
            MongoUrl mongoUrl = new MongoUrl(_MongoDbConnectionStr);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            return database.GetCollection<T>(collectionName ?? typeof(T).Name);
        }

        private static IMongoCollection<BsonDocument> GetBsonCollection<T>(string collectionName = null)
        {
            MongoUrl mongoUrl = new MongoUrl(_MongoDbConnectionStr);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            return database.GetCollection<BsonDocument>(collectionName ?? typeof(T).Name);
        }
    }
}
