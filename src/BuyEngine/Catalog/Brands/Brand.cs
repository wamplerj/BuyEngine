namespace BuyEngine.Catalog.Brands;

public record Brand
{
    public Guid Id { get; set; }
    public string Name { get; init; }
    public string WebsiteUrl { get; init; }
    public string Notes { get; init; }
}