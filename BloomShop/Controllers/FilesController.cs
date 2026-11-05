using Microsoft.AspNetCore.Mvc;
using BloomShop.Models;

namespace BloomShop.Controllers;

public class FilesController : Controller
{
    private readonly IWebHostEnvironment _env;
    private const long MaxBytes = 10 * 1024 * 1024;
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
        { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf", ".txt", ".doc", ".docx", ".zip" };

    public FilesController(IWebHostEnvironment env) => _env = env;

    private string Root => Path.Combine(_env.WebRootPath, "Files");
    private void Ensure() { if (!Directory.Exists(Root)) Directory.CreateDirectory(Root); }

    private List<FileItemViewModel> ListFiles() =>
        Directory.GetFiles(Root).Select(p =>
        {
            var i = new FileInfo(p);
            return new FileItemViewModel
            {
                Name = i.Name, RelativePath = $"/Files/{i.Name}", SizeBytes = i.Length,
                UploadedAt = i.CreationTime, Extension = i.Extension
            };
        }).OrderByDescending(f => f.UploadedAt).ToList();

    [HttpGet]
    public IActionResult Index()
    {
        Ensure();
        ViewBag.Files = ListFiles();
        return View(new FileUploadViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken, RequestSizeLimit(MaxBytes)]
    public async Task<IActionResult> Upload(FileUploadViewModel model)
    {
        Ensure();
        if (model.File == null || model.File.Length == 0)
        {
            ModelState.AddModelError(nameof(model.File), "Choose a file");
            ViewBag.Files = ListFiles();
            return View("Index", model);
        }
        var ext = Path.GetExtension(model.File.FileName);
        if (!Allowed.Contains(ext) || model.File.Length > MaxBytes)
        {
            ModelState.AddModelError(nameof(model.File), "Invalid type or file too large (max 10 MB)");
            ViewBag.Files = ListFiles();
            return View("Index", model);
        }
        var name = Path.GetFileName(model.File.FileName);
        name = string.Join("_", name.Split(Path.GetInvalidFileNameChars()));
        var dest = Path.Combine(Root, name);
        if (System.IO.File.Exists(dest))
            dest = Path.Combine(Root, $"{Path.GetFileNameWithoutExtension(name)}_{DateTime.Now:yyyyMMddHHmmss}{ext}");
        await using (var s = new FileStream(dest, FileMode.Create))
            await model.File.CopyToAsync(s);
        TempData["Success"] = "Uploaded";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public new IActionResult View(string name)
    {
        name = Path.GetFileName(name);
        var path = Path.Combine(Root, name);
        if (!System.IO.File.Exists(path)) return NotFound();
        var i = new FileInfo(path);
        var model = new FileItemViewModel
        {
            Name = i.Name, RelativePath = $"/Files/{i.Name}", SizeBytes = i.Length,
            UploadedAt = i.CreationTime, Extension = i.Extension
        };
        var textExt = new[] { ".txt", ".md", ".csv", ".json" };
        if (textExt.Contains(i.Extension, StringComparer.OrdinalIgnoreCase) && i.Length < 200000)
            ViewBag.TextContent = System.IO.File.ReadAllText(path);
        return View(model);
    }
}
