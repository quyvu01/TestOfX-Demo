using Kernel.Attributes;
using OfX.Abstractions;

namespace Service2.Contract.Queries;

public sealed record GetUserOfXQuery(List<string> SelectorIds, string Expression)
    : DataMappableOf<UserOfAttribute>(SelectorIds, Expression);