using MassTransit;
using OfX.Abstractions;
using Service2.Contract.Queries;
using WorkerService1.Models;

namespace WorkerService1.Consumers;

public class Worker1Consumer(IQueryOfHandler<User, GetUserOfXQuery> userQueryOfHandler) : IConsumer<GetUserOfXQuery>
{
    public async Task Consume(ConsumeContext<GetUserOfXQuery> context)
    {
        var users = await userQueryOfHandler.GetDataAsync(new OfXRequestContext(context.Message, [],
            context.CancellationToken));
        await context.RespondAsync(users);
    }
}

public sealed class OfXRequestContext(
    GetUserOfXQuery query,
    Dictionary<string, string> headers,
    CancellationToken cancellationToken)
    : RequestContext<GetUserOfXQuery>
{
    public Dictionary<string, string> Headers { get; } = headers;
    public CancellationToken CancellationToken { get; } = cancellationToken;
    public GetUserOfXQuery Query { get; } = query;
}