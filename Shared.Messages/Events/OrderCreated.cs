using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events
{
    public record OrderCreated(
        int OrderId,
        int ProductId,
        int Quantity,
        decimal Price,
        DateTime CreatedAt
    );
}
