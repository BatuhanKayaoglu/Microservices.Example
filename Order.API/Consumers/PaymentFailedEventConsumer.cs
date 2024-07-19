using MassTransit;
using Order.API.Models;
using Order.API.Models.Enums;
using Shared.Events.Common;

namespace Order.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        readonly OrderAPIDbContext _context;
        public PaymentFailedEventConsumer(OrderAPIDbContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            // OrderId'ye göre Order'ın status'ünü Completed yap
            Models.Entities.Order? order = _context.Orders.Find(context.Message.OrderId);
            order.OrderStatus = OrderStatus.Failed;
            await _context.SaveChangesAsync();
        }
    }
}
