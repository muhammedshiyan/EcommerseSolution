using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Clients;
using OrderService.Data;
using OrderService.Models;
namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ProductClient _productClient;

        public OrdersController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> CreateOrder(int productId)
        {
            var product = await _productClient.GetProductByIdAsync(productId);

            if (product == null)
                return NotFound("Product not found");

            // Fake order creation logic
            return Ok(new
            {
                OrderId = Guid.NewGuid(),
                Product = product,
                Status = "Created"
            });
        }

        [HttpGet("with-products")]
        public async Task<IActionResult> GetOrdersWithProducts()
        {
            var client = _httpClientFactory.CreateClient("ApiGateway");

            // Call ProductService via Gateway
            var products = await client.GetFromJsonAsync<List<ProductDto>>("/products");

            // Mock order data + include product info
            var orders = new[]
            {
                new { OrderId = 1, Product = products?.FirstOrDefault(), Quantity = 2 },
                new { OrderId = 2, Product = products?.Skip(1).FirstOrDefault(), Quantity = 1 }
            };

            return Ok(orders);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id) return BadRequest();

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
