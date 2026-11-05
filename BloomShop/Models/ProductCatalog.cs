namespace BloomShop.Models;

public static class ProductCatalog
{
    public static List<Product> All { get; } =
    [
        new() { Id = 1, NameEn = "Silk Midi Dress", NameUk = "Шовкова міді-сукня", Category = "Dresses", Price = 1890, Color = "Blush", Emoji = "👗",
            DescriptionEn = "Fluid silk midi with soft drape — perfect for evening and brunch.", DescriptionUk = "Шовкова міді з м'яким силуетом — для вечора та бранчу." },
        new() { Id = 2, NameEn = "Cashmere Cardigan", NameUk = "Кашеміровий кардиган", Category = "Knitwear", Price = 2450, Color = "Ivory", Emoji = "🧥",
            DescriptionEn = "Lightweight cashmere layer for cool mornings.", DescriptionUk = "Легкий кашемір на прохолодний ранок." },
        new() { Id = 3, NameEn = "Wide Linen Pants", NameUk = "Широкі лляні штани", Category = "Bottoms", Price = 1290, Color = "Sand", Emoji = "👖",
            DescriptionEn = "Breathable linen with high waist and wide leg.", DescriptionUk = "Дихаючий лен, висока талія, широкий крій." },
        new() { Id = 4, NameEn = "Satin Camisole", NameUk = "Атласна майка", Category = "Tops", Price = 790, Color = "Champagne", Emoji = "👚",
            DescriptionEn = "Minimal satin top — wear alone or under a blazer.", DescriptionUk = "Мінімалістичний топ — сам або під жакет." },
        new() { Id = 5, NameEn = "Trench Coat", NameUk = "Тренч", Category = "Outerwear", Price = 3290, Color = "Camel", Emoji = "🧥",
            DescriptionEn = "Classic trench with belt — rain or shine.", DescriptionUk = "Класичний тренч із поясом — у будь-яку погоду." },
        new() { Id = 6, NameEn = "Pleated Skirt", NameUk = "Плісирована спідниця", Category = "Bottoms", Price = 990, Color = "Black", Emoji = "👗",
            DescriptionEn = "Midi pleats that move with every step.", DescriptionUk = "Міді в складку — рухається з кожним кроком." },
    ];

    public static Product? Get(int id) => All.FirstOrDefault(p => p.Id == id);
}
