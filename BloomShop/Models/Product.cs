namespace BloomShop.Models;

public class Product
{
    public int Id { get; set; }
    public string NameEn { get; set; } = "";
    public string NameUk { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public string Color { get; set; } = "";
    public string DescriptionEn { get; set; } = "";
    public string DescriptionUk { get; set; } = "";
    public string Emoji { get; set; } = "👗";
}
