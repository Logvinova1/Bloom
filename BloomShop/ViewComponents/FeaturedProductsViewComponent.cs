using Microsoft.AspNetCore.Mvc;
using BloomShop.Models;

namespace BloomShop.ViewComponents;

/// <summary>
/// ViewComponent: shows a small selection of featured products (Bloom shop).
/// Invoke from any view: @await Component.InvokeAsync("FeaturedProducts")
/// Optional: @await Component.InvokeAsync("FeaturedProducts", new { count = 2 })
/// </summary>
public class FeaturedProductsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(int count = 3)
    {
        if (count < 1) count = 1;
        if (count > 6) count = 6;

        var items = ProductCatalog.All.Take(count).ToList();
        return View(items); // Views/Shared/Components/FeaturedProducts/Default.cshtml
    }
}
