using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OrderService.Models;
using static OrderService.Clients.models;

namespace OrderService.Clients
{
    public class ProductClient
    {
        private readonly HttpClient _httpClient;

        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Product>(
                $"http://localhost:5024/products/{id}"); // call via ApiGateway
        }
    }
}
