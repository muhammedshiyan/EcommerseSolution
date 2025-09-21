using System;
using System.Text.Json.Serialization;

namespace OrderService.Models
{
    public record ProductDto
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; init; }

        [JsonPropertyName("productName")]
        public string ProductName { get; init; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; init; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; init; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; init; }

        [JsonPropertyName("currency")]
        public string Currency { get; init; } = "USD";

        [JsonPropertyName("availableStock")]
        public int AvailableStock { get; init; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; init; } = true;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

        [JsonPropertyName("tags")]
        public string[] Tags { get; init; } = Array.Empty<string>();
    }
}
