using Basket.API.Data;
using Basket.API.Models;

using BuildingBlocks.Behaviour;

using Carter;

using Marten;

var builder = WebApplication.CreateBuilder(args);

// add services to the container
var assembly = typeof(Program).Assembly;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.AddMarten(options =>
{
    options.DatabaseSchemaName = "dbo";
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

var app = builder.Build();

app.MapCarter();

app.MapGet("/", () => "Hello World!");

app.Run();
