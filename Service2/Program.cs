using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using OfX.EntityFrameworkCore.Extensions;
using OfX.Extensions;
using OfX.Grpc.Extensions;
using Service2.Contract;
using WorkerService1;
using WorkerService1.Contexts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOfX(cfg =>
    {
        cfg.AddContractsContainNamespaces(typeof(IService2ContractAssembly).Assembly);
        cfg.AddHandlersFromNamespaceContaining<IAssemblyMarker>();
    })
    .AddOfXEFCore<Service2Context>();

builder.Services.AddDbContextPool<Service2Context>(options =>
{
    options.UseNpgsql("Host=localhost;Username=postgres;Password=Abcd@2021;Database=OfXTestService2", b =>
    {
        b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
    });
}, 128);
builder.Services.AddGrpc();

var app = builder.Build();
app.MapOfXGrpcService();
app.Run();