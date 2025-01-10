using BuildingBlocks.Behaviour;
using BuildingBlocks.Exceptions.Handler;

using Catalog.API.Data;

var builder = WebApplication.CreateBuilder(args);

//add services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMarten(options => 
{
    options.DatabaseSchemaName = "dbo";
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<InitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

//builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

//congfigure the http request pipeline
app.MapCarter();
app.UseHealthChecks("/health");
app.UseExceptionHandler(options => { });

app.Run();
