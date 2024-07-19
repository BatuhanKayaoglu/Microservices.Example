using MassTransit;
using Order.API.Models;
using Order.API.Models.Enums;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderAPIDbContext _context;
        public PaymentCompletedEventConsumer(OrderAPIDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            // OrderId'ye göre Order'ın status'ünü Completed yap
            Models.Entities.Order? order = _context.Orders.Find(context.Message.OrderId);
            order.OrderStatus = OrderStatus.Completed;
            await _context.SaveChangesAsync();
        }
    }
}
