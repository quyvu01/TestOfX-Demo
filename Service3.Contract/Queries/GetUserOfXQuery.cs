using Kernel.Attributes;
using OfX.Abstractions;

namespace Service3.Contract.Queries;

public sealed record GetProvinceOfXQuery(List<string> SelectorIds, string Expression)
    : DataMappableOf<ProvinceOfAttribute>(SelectorIds, Expression);