using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events
{
    public record UserRegistered(
        int UserId,
        string Username,
        string Email,
        DateTime RegisteredAt
    );
}
