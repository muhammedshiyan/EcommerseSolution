using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Contracts
{
    public record OrderCreated
    {
        public int OrderId { get; init; }
        public int ProductId { get; init; }
        public int Quantity { get; init; }
        public decimal Total { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
