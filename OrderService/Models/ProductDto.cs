using System;
using System.Text.Json.Serialization;

namespace OrderService.Models
{
    public record ProductDto
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; init; }
    }
}
