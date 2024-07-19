using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventCustomer : IConsumer<OrderCreatedEvent>
    {
        private readonly MongoDBService _mongoDBService;
        IMongoCollection<Stock.API.Models.Entities.Stock> stockCollection;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public OrderCreatedEventCustomer(MongoDBService mongoDBService, ISendEndpointProvider sendEndpointProvider)
        {
            _mongoDBService = mongoDBService;
            stockCollection = _mongoDBService.GetCollection<Stock.API.Models.Entities.Stock>();
            _sendEndpointProvider = sendEndpointProvider;
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

                StockReservedEvent stockReservedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    BuyerId = context.Message.BuyerId,
                    TotalPrice = context.Message.TotalPrice
                };

                // Payment event starting here.    
                ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
                await sendEndpoint.Send(stockReservedEvent);  
                // Publish yaparsak tüm servislerdeki consumerlar tetiklenir. Send ise sadece bir consumer tetikler.
                // Publish'de merkezdeki evente abone olan tüm consumerlar tetiklenir.

                // Send'de ise event merkezli değil kuyruk merkezli bir yapı oluşturulur.İlgili event hangi kuyruga yayınlanıyorsa o kuyrugu dinleyen servislere o event gönderilecektir.
                // Yani sadece bir consumer tetiklenir. Bu durumda Payment servisindeki StockReservedEventConsumer tetiklenir.   
                // Payment'i tetikleyecek başka bir service olamayacagından dolayı send kullanılabilir. 
            }
            else
            {

            }
        }
    }
}
