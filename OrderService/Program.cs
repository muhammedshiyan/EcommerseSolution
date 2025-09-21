using Microsoft.EntityFrameworkCore;
using OrderService.Clients;
using OrderService.Data;
using OrderService.Services;
using Polly;
using Shared.Messages;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext> (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("ApiGateway", client =>
{
    client.BaseAddress = new Uri("http://localhost:5024"); // Gateway URL
}).AddTransientHttpErrorPolicy(p => p.RetryAsync(3))
.AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
builder.Services.AddHttpClient<ProductClient>();
builder.Services.AddScoped<OrderProcessor>();
builder.Services.AddScoped<IOrderProcessor, OrderProcessor>();
//builder.Services.AddSingleton<IMessageBus, InMemoryMessageBus>();
//if (builder.Environment.IsDevelopment())
//{
//    builder.Services.AddSingleton<IMessageBus, InMemoryMessageBus>();
//}
//else
//{
//    builder.Services.AddSingleton<IMessageBus>(_ =>
//        new RabbitMqMessageBus("localhost", "ecommerce_exchange"));
//}
builder.Services.AddSingleton<IMessageBus>(_ => new RabbitMqMessageBus("localhost", "ecommerce_exchange"));

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();


app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
