namespace Shared.Messages
{
    public class OrderCreated
    {
        private DateTime createdAt;

        public OrderCreated(int OrderId, int ProductId, int Quantity, decimal Price, DateTime CreatedAt)
        {
            this.OrderId = OrderId;
            this.ProductId = ProductId;
            this.Quantity = Quantity;
            this.Price = Price;
            createdAt = CreatedAt;
        }

        public int OrderId { get; }
        public int ProductId { get; }
        public int Quantity { get; }
        public decimal Price { get; }
        public DateTime CreatedAt { get; }
    }
}