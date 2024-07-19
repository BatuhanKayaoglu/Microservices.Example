using MassTransit;
using Order.API.Models.Enums;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {

        readonly OrderAPIDbContext _context;
        public StockNotReservedEventConsumer(OrderAPIDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            // OrderId'ye göre Order'ın status'ünü Failed yap
            Models.Entities.Order? order = _context.Orders.Find(context.Message.OrderId);
            order.OrderStatus = OrderStatus.Failed;
            await _context.SaveChangesAsync();
        }
    }
}
