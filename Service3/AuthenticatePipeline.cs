using OfX.Abstractions;
using OfX.Attributes;
using OfX.Responses;

namespace Service3Api;

public class AuthenticatePipeline<TAttribute> : IReceivedPipelineBehavior<TAttribute> where TAttribute : OfXAttribute
{
    public async Task<ItemsResponse<OfXDataResponse>> HandleAsync(RequestContext<TAttribute> requestContext,
        Func<Task<ItemsResponse<OfXDataResponse>>> next)
    {
        var isAuthen = requestContext.Headers.ContainsKey("Abcd");
        if (!isAuthen)
            return new ItemsResponse<OfXDataResponse>([]);
        var result = await next.Invoke();
        return result;
    }
}