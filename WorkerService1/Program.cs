using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OfX.EntityFrameworkCore.Extensions;
using OfX.Extensions;
using Service2.Contract;
using WorkerService1;
using WorkerService1.Consumers;
using WorkerService1.Contexts;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOfX(cfg =>
{
    cfg.RegisterContractsContainsAssemblies(typeof(IService2ContractAssembly).Assembly);
    cfg.RegisterHandlersContainsAssembly<IAssemblyMarker>();
}).RegisterOfXEntityFramework<Service2Context, IAssemblyMarker>();

builder.Services.AddDbContextPool<Service2Context>(options =>
{
    options.UseNpgsql("Host=localhost;Username=postgres;Password=Abcd@2021;Database=OfXTestService1", b =>
    {
        b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
    });
}, 128);

builder.Services.AddMassTransit(configurator =>
{
    configurator.SetKebabCaseEndpointNameFormatter();
    configurator.AddConsumer<Worker1Consumer>();
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

var host = builder.Build();
host.Run();