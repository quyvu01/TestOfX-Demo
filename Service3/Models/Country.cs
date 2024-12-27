namespace Service3Api.Models;

public class Country
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<Province> Provinces { get; set; }
}