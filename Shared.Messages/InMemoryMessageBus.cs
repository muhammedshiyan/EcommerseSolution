using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public class InMemoryMessageBus : IMessageBus
    {
        public Task PublishAsync<T>(T message) where T : class
        {
            Console.WriteLine($"[InMemoryBus] Published: {message}");
            return Task.CompletedTask;
        }
    }
}
