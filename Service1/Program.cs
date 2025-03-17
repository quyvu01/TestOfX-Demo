using System.Reflection;
using Kernel;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OfX.EntityFrameworkCore.Extensions;
using OfX.Extensions;
using OfX.HotChocolate.Extensions;
using OfX.MongoDb.Extensions;
using OfX.Nats.Extensions;
using Serilog;
using Service1;
using Service1.Contexts;
using Service1.GraphQls;
using Service1.Models;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("Service1MongoDb");
var memberSocialCollection = database.GetCollection<MemberSocial>("MemberSocials");

var registerBuilder = builder.Services.AddGraphQLServer()
    .AddQueryType<Query>();

builder.Services.AddOfX(cfg =>
    {
        cfg.AddAttributesContainNamespaces(typeof(IKernelAssemblyMarker).Assembly);
        // cfg.AddRabbitMq(config => config.Host("localhost", "/"));
        cfg.AddModelConfigurationsFromNamespaceContaining<IAssemblyMarker>();
        cfg.AddNats(config => config.Url("nats://localhost:4222"));
        cfg.ThrowIfException();
        cfg.SetMaxObjectSpawnTimes(16);
    })
    .AddOfXEFCore(cfg => cfg.AddDbContexts(typeof(Service1Context), typeof(OtherService1Context)))
    .AddMongoDb(cfg => cfg.AddCollection(memberSocialCollection))
    .AddHotChocolate(cfg => cfg.AddRequestExecutorBuilder(registerBuilder));

builder.Services.AddDbContextPool<Service1Context>(options =>
{
    options.UseNpgsql("Host=localhost;Username=postgres;Password=Abcd@2021;Database=OfXTestService1", b =>
    {
        b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
    });
}, 128);

builder.Services.AddDbContextPool<OtherService1Context>(options =>
{
    options.UseNpgsql("Host=localhost;Username=postgres;Password=Abcd@2021;Database=OfXTestOtherService1", b =>
    {
        b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
    });
}, 128);


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGraphQL();

app.Run();