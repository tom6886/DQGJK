using MongoDB.Driver;
using System.Configuration;

namespace DQGJK.Winform
{
    internal class MongoHandler
    {
        private static string _MongoDbConnectionStr = ConfigurationManager.AppSettings["mongodb"];

        internal static void Save<T>(T t)
        {
            GetCollection<T>().InsertOne(t);
        }

        private static IMongoCollection<T> GetCollection<T>(string collectionName = null)
        {
            MongoUrl mongoUrl = new MongoUrl(_MongoDbConnectionStr);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            return database.GetCollection<T>(collectionName ?? typeof(T).Name);
        }
    }
}
