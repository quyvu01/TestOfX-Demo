using System.Linq.Expressions;
using OfX.EntityFrameworkCore;
using OfX.Responses;
using Service3.Contract.Queries;
using Service3Api.Models;

namespace Service3Api.Handlers;

public sealed class ProvinceOfXHandler(IServiceProvider serviceProvider)
    : EfQueryOfXHandler<Province, GetProvinceOfXQuery>(serviceProvider)
{
    protected override Func<GetProvinceOfXQuery, Expression<Func<Province, bool>>> SetFilter() =>
        q => u => q.SelectorIds.Contains(u.Id);

    protected override Expression<Func<Province, OfXDataResponse>> SetHowToGetDefaultData() =>
        u => new OfXDataResponse { Id = u.Id, Value = u.Name };
}