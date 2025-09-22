using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messages;
using Shared.Messages.Events;
using System.Text;
using System.Text.Json;

namespace ProductService.Consumers;

public class OrderCreatedConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public OrderCreatedConsumer()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "ecommerce_exchange", type: ExchangeType.Fanout);

        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: "ecommerce_exchange", routingKey: "");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var evt = JsonSerializer.Deserialize<OrderCreated>(json);

            Console.WriteLine($"[ProductService] Received OrderCreated event: {evt?.OrderId}");
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // No background loop needed since RabbitMQ consumer runs on events
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
