using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.Models.Entities;
using Order.API.Models.Enums;
using Order.API.ViewModels;
using Shared.Events;
using Shared.Messages;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly OrderAPIDbContext _db;
        private readonly IPublishEndpoint publishEndpoint;

        public OrdersController(OrderAPIDbContext db, IPublishEndpoint publishEndpoint)
        {
            _db = db;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderVM createOrder)
        {
            Order.API.Models.Entities.Order order = new()
            {
                OrderId = Guid.NewGuid(),
                BuyerId = createOrder.BuyerId,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatus.Suspend
            };

            order.OrderItems = createOrder.OrderItems.Select(oi => new OrderItem
            {
                Count = oi.Count,
                Price = oi.Price,
                ProductId = oi.ProductId
            }).ToList();

            order.TotalPrice = order.OrderItems.Sum(oi => oi.Price * oi.Count);

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                OrderId = order.OrderId,
                BuyerId = order.BuyerId,
                OrderItems = order.OrderItems.Select(oi => new OrderItemMessage
                {
                    Count = oi.Count,
                    ProductId = oi.ProductId
                }).ToList()
            };

            await publishEndpoint.Publish(orderCreatedEvent);

            return Ok();
        }
    }
}
