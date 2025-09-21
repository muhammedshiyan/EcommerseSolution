using OrderService.Data;
using OrderService.Models;
using Shared.Messages;
using Shared.Messages.Events;
using System.Net.Http;
using System.Text.Json;

namespace OrderService.Services;

public interface IOrderProcessor
{
    Task PlaceOrderAsync(int productId, int quantity);
}

public class OrderProcessor : IOrderProcessor
{

    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMessageBus _messageBus;
    public OrderProcessor(AppDbContext context, IHttpClientFactory httpClientFactory, IMessageBus messageBus)
    {

        _messageBus = messageBus;
    }


    public async Task PlaceOrderAsync(int productId, int quantity)
    {
        var client = _httpClientFactory.CreateClient("ApiGateway");
        var response = await client.GetAsync($"/products/{productId}");
        if (!response.IsSuccessStatusCode) throw new Exception("Product not found");

        var product = JsonSerializer.Deserialize<ProductDto>(await response.Content.ReadAsStringAsync());

        // Create order
        var order = new Order
        {
            ProductId = productId,
            Quantity = quantity,
            Id = product.ProductId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Publish event to RabbitMQ
        var orderCreated = new OrderCreated(
            OrderId: order.Id,
            ProductId: productId,
            Quantity: quantity,
            Price: product.Price,
            CreatedAt: DateTime.UtcNow
        );

        await _messageBus.PublishAsync(orderCreated);
        Console.WriteLine($"[OrderService] Published OrderCreated event: {order.Id}");
    }
}
