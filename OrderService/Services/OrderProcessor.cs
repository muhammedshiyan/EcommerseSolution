using OrderService.Data;
using OrderService.Models;
using Shared.Messages;
using Shared.Messages.Events;
using System.Net.Http.Json;
using System.Text.Json;

namespace OrderService.Services
{
    public interface IOrderProcessor
    {
        Task PlaceOrderAsync(int productId, int quantity);
    }

    public class OrderProcessor : IOrderProcessor
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMessageBus _messageBus;
        private readonly ILogger<OrderProcessor> _logger;

        public OrderProcessor(AppDbContext context, IHttpClientFactory httpClientFactory, IMessageBus messageBus, ILogger<OrderProcessor> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _messageBus = messageBus;
            _logger = logger;
        }

        public async Task PlaceOrderAsync(int productId, int quantity)
        {
            _logger.LogInformation("Placing order for ProductId: {ProductId}, Quantity: {Quantity}", productId, quantity);

            if (productId <= 0 || quantity <= 0)
                throw new ArgumentException("Invalid product ID or quantity");

            var client = _httpClientFactory.CreateClient("ApiGateway");
            client.DefaultRequestHeaders.Add("Cookie", "m=59b9:true");
            var response = await client.GetAsync($"/products");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Product not found for ProductId: {ProductId}", productId);
                throw new NotFoundException("Product not found");
            }
            var jsonContent = await response.Content.ReadAsStringAsync();
            //var product ; // JsonSerializer.Deserialize<ProductDto>(jsonContent);
            //if (product == null)
            //{
            //    _logger.LogError("Failed to deserialize product for ProductId: {ProductId}", productId);
            //    throw new NotFoundException("Product data invalid");
            //}

            var order = new Order
            {
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = 1, //product.ProductId, // Verify this logic aligns with your domain //
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            //var orderCreated = new OrderCreated(
            //    OrderId: order.Id,
            //    ProductId: productId,
            //    Quantity: quantity,
            //    Price:10 ,// product.Price,
            //    CreatedAt: DateTime.UtcNow
            //);

 var orderCreated = new OrderCreated(
    OrderId: 1,
    ProductId: 1,
    Quantity: 1,
    Price: 10,// product.Price,
    CreatedAt: DateTime.UtcNow
);

            await _messageBus.PublishAsync(orderCreated);
            _logger.LogInformation("Order created and event published: OrderId: {OrderId}", order.Id);
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}