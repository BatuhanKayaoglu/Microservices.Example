using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventCustomer : IConsumer<OrderCreatedEvent>
    {
        private readonly MongoDBService _mongoDBService;
        IMongoCollection<Stock.API.Models.Entities.Stock> stockCollection;

        public OrderCreatedEventCustomer(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
            stockCollection = _mongoDBService.GetCollection<Stock.API.Models.Entities.Stock>();
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
            {
                // Check if there is enough stock for each product in the order and add the result to the stockResult list
                // eğer orderItems içersindeki ürünlerden biri dahi yetersizse stockResult listesine false eklenecek ve bu order iptal edilecek 
                stockResult.Add((await stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).Any());
            }

            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (OrderItemMessage orderItem in context.Message.OrderItems)
                {
                    int stock = stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId).Result.FirstOrDefault().Count -= orderItem.Count;
                    await stockCollection.UpdateOneAsync(s => s.ProductId == orderItem.ProductId, Builders<Stock.API.Models.Entities.Stock>.Update.Set(s => s.Count, stock));
                }
                // Payment event starting here.    
            }
            else
            {

            }
        }
    }
}
