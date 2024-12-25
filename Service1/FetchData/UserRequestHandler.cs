using Kernel.Attributes;
using MassTransit;
using OfX.Abstractions;
using OfX.Responses;
using Service2.Contract.Queries;

namespace Service1.FetchData;

public sealed class UserRequestHandler(IRequestClient<GetUserOfXQuery> client)
    : IMappableRequestHandler<GetUserOfXQuery, UserOfAttribute>
{
    public async Task<ItemsResponse<OfXDataResponse>> RequestAsync(GetUserOfXQuery request,
        CancellationToken cancellationToken = new())
    {
        var users = await client.GetResponse<ItemsResponse<OfXDataResponse>>(request, cancellationToken);
        return users.Message;
    }
}