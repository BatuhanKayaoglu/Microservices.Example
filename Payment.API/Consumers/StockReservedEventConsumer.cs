using MassTransit;
using Shared.Events;
using Shared.Events.Common;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId,  
                };      

                publishEndpoint.Publish(paymentCompletedEvent); 
                Console.WriteLine("Payment completed event published and successfull"); 
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Payment failed"
                };    
                
                publishEndpoint.Publish(paymentFailedEvent);    
                Console.WriteLine("Payment failed event published and successfull");    
            }

            return Task.CompletedTask;  
        }
    }
}
