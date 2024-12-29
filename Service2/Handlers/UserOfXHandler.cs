using System.Linq.Expressions;
using Kernel.Attributes;
using OfX.Abstractions;
using OfX.EntityFrameworkCore;
using OfX.Responses;
using WorkerService1.Models;

namespace WorkerService1.Handlers;

public sealed class UserOfXHandler(IServiceProvider serviceProvider)
    : EfQueryOfXHandler<User, UserOfAttribute>(serviceProvider)
{
    protected override Func<RequestOf<UserOfAttribute>, Expression<Func<User, bool>>> SetFilter() =>
        q => u => q.SelectorIds.Contains(u.Id);

    protected override Expression<Func<User, OfXDataResponse>> SetHowToGetDefaultData() =>
        u => new OfXDataResponse { Id = u.Id, Value = u.Name };
}