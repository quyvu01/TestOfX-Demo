using System.Reflection;
using Kernel;
using Microsoft.EntityFrameworkCore;
using OfX.EntityFrameworkCore.Extensions;
using OfX.Extensions;
using OfX.Nats.Extensions;
using Service3Api;
using Service3Api.Contexts;
using Service3Api.Converters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOfX(cfg =>
    {
        cfg.AddAttributesContainNamespaces(typeof(IKernelAssemblyMarker).Assembly);
        // cfg.AddRabbitMq(config => config.Host("localhost", "/"));
        cfg.AddNats(config => config.Url("nats://localhost:4222"));
        cfg.AddModelConfigurationsFromNamespaceContaining<IAssemblyMarker>();
        cfg.AddStronglyTypeIdConverter(c => c.OfType<IdConverterRegister>());
    })
    .AddOfXEFCore(cfg => cfg.AddDbContexts(typeof(Service3Context)));

builder.Services.AddDbContextPool<Service3Context>(options =>
{
    options.UseNpgsql("Host=localhost;Username=postgres;Password=Abcd@2021;Database=OfXTestService3", b =>
    {
        b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
    });
}, 128);

var app = builder.Build();

app.Run();