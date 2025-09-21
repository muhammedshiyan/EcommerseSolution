using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message) where T : class;
    }
}
