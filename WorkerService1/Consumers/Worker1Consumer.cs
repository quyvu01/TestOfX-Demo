using MassTransit;
using OfX.Abstractions;
using Service2.Contract.Queries;
using WorkerService1.Models;

namespace WorkerService1.Consumers;

public class Worker1Consumer(IQueryOfHandler<User, GetUserOfXQuery> userQueryOfHandler) : IConsumer<GetUserOfXQuery>
{
    public async Task Consume(ConsumeContext<GetUserOfXQuery> context)
    {
        var users = await userQueryOfHandler.GetDataAsync(context.Message, context.CancellationToken);
        await context.RespondAsync(users);
    }
}