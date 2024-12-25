using MassTransit;
using OfX.Extensions;
using Service1;
using Service1.Contract;
using Service2.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOfX(cfg =>
{
    cfg.RegisterContractsContainsAssemblies(typeof(IService1ContractAssembly).Assembly, typeof(IService2ContractAssembly).Assembly);
    cfg.RegisterHandlersContainsAssembly<IAssemblyMarker>();
});

builder.Services.AddMassTransit(configurator =>
{
    configurator.SetKebabCaseEndpointNameFormatter();
    configurator.AddConsumersFromNamespaceContaining<IAssemblyMarker>();
    configurator.UsingRabbitMq((context, bus) =>
    {
        bus.Host("localhost", "/", c =>
        {
            c.Username("guest");
            c.Password("guest");
        });
        bus.ConfigureEndpoints(context);

    });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();