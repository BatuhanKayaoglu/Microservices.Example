using MassTransit;
using Payment.API.Consumers;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<StockReservedEventConsumer>();  // consumer bilgisini ekledik

    configurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetSection("RabbitMQUrl").Value);

        cfg.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e =>
        {
            e.ConfigureConsumer<StockReservedEventConsumer>(context); // RabbitMQ išersinde hangi kuyrugu dinleyecegini belirtmeliyiz.
        });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
