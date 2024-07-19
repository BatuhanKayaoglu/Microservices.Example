using MongoDB.Driver;

namespace Stock.API.Services
{
    public class MongoDBService
    {
        private readonly IMongoDatabase mongoDatabase;

        public MongoDBService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            mongoDatabase = client.GetDatabase("StockAPIDB");        
        }

        // okumak istediğimiz tablolara göre methodlar yazıcaz.
        public IMongoCollection<T> GetCollection<T>() => mongoDatabase.GetCollection<T>(typeof(T).Name.ToLowerInvariant()); 
    }
}
