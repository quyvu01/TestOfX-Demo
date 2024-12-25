using System.Linq.Expressions;
using OfX.EntityFrameworkCore;
using OfX.Responses;
using Service2.Contract.Queries;
using WorkerService1.Models;

namespace WorkerService1.Handlers;

public sealed class UserOfXHandler(IServiceProvider serviceProvider)
    : EfQueryOfXHandler<User, GetUserOfXQuery>(serviceProvider)
{
    protected override Func<GetUserOfXQuery, Expression<Func<User, bool>>> SetFilter() =>
        q => u => q.SelectorIds.Contains(u.Id);

    protected override Expression<Func<User, OfXDataResponse>> SetHowToGetDefaultData() =>
        u => new OfXDataResponse { Id = u.Id, Value = u.Name };
}