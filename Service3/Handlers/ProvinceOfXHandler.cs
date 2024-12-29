using System.Linq.Expressions;
using Kernel.Attributes;
using OfX.Abstractions;
using OfX.EntityFrameworkCore;
using OfX.Responses;
using Service3Api.Models;

namespace Service3Api.Handlers;

public sealed class ProvinceOfXHandler(IServiceProvider serviceProvider)
    : EfQueryOfXHandler<Province, ProvinceOfAttribute>(serviceProvider)
{
    protected override Func<RequestOf<ProvinceOfAttribute>, Expression<Func<Province, bool>>> SetFilter() =>
        q => u => q.SelectorIds.Contains(u.Id);

    protected override Expression<Func<Province, OfXDataResponse>> SetHowToGetDefaultData() =>
        u => new OfXDataResponse { Id = u.Id, Value = u.Name };
}