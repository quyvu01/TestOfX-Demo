using OfX.Extensions;
using OfX.Grpc.Extensions;
using Service1;
using Service1.Contract;
using Service2.Contract;
using Service3.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOfX(cfg =>
{
    cfg.AddContractsContainNamespaces(typeof(IService1ContractAssembly).Assembly,
        typeof(IService2ContractAssembly).Assembly, typeof(IService3ContractAssembly).Assembly);
    cfg.AddHandlersFromNamespaceContaining<IAssemblyMarker>();
    cfg.AddGrpcClients(config => config
        .RegisterContractsFromNamespaceContainning<IService2ContractAssembly>("http://localhost:5001")
        .RegisterContractsFromNamespaceContainning<IService3ContractAssembly>("http://localhost:5013")
    );
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