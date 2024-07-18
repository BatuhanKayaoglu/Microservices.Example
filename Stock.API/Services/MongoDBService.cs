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
    }
}
