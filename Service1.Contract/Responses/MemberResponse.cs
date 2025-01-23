using Kernel.Attributes;
using Kernel.ModelIds;

namespace Service1.Contract.Responses;

public class MemberResponse
{
    public string Id { get; set; }
    public string UserId { get; set; }
    [UserOf(nameof(UserId))] public string UserName { get; set; }

    [UserOf(nameof(UserId), Expression = "Email")]
    public string UserEmail { get; set; }

    [UserOf(nameof(UserId), Expression = "ProvinceId")]
    public string ProvinceId { get; set; }

    [ProvinceOf(nameof(ProvinceId), Expression = "CountryId", Order = 1)]
    public StronglyTypedId<string> CountryId { get; set; }

    [CountryOf(nameof(CountryId), Expression = "Provinces[asc Name]", Order = 2)]
    public List<ProvinceResponse> Provinces { get; set; }
}