using Microsoft.AspNetCore.Mvc;
using BloomShop.Models;

namespace BloomShop.Controllers;

public class CatalogController : Controller
{
    public IActionResult Index()
    {
        return View(ProductCatalog.All);
    }

    public IActionResult Details(int id)
    {
        var p = ProductCatalog.Get(id);
        if (p == null) return NotFound();
        return View(p);
    }
}
