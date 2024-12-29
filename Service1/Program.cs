using Kernel;
using Kernel.Attributes;
using OfX.Extensions;
using OfX.Grpc.Extensions;
using Service1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOfX(cfg =>
{
    cfg.AddAttributesContainNamespaces(typeof(IKernelAssemblyMarker).Assembly);
    cfg.AddHandlersFromNamespaceContaining<IAssemblyMarker>();
    cfg.AddGrpcClients(config => config
        .AddGrpcHostWithOfXAttributes("http://localhost:5001", [typeof(UserOfAttribute)])
        .AddGrpcHostWithOfXAttributes("http://localhost:5013", [typeof(ProvinceOfAttribute)])
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