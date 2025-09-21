using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Shared.Messages;

public class RabbitMqMessageBus : IMessageBus, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    public string ExchangeName { get; }
    public RabbitMqMessageBus(string hostname = "localhost", string exchangeName = "ecommerce_exchange")
    {
        var factory = new ConnectionFactory() { HostName = hostname };

        ExchangeName = exchangeName;
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare a fanout exchange so all services can listen
        _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

    }



    public Task PublishAsync<T>(T message) where T : class
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: "",
            basicProperties: null,
            body: body
        );

        Console.WriteLine($"[RabbitMqBus] Published: {typeof(T).Name} => {json}");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
