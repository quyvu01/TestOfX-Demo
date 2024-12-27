namespace Service3Api.Models;

public sealed class Province
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string CountryId { get; set; }
    public Country Country { get; set; }
}