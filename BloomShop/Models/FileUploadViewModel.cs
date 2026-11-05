using System.ComponentModel.DataAnnotations;

namespace BloomShop.Models;

public class FileUploadViewModel
{
    [Required]
    public IFormFile? File { get; set; }
}

public class FileItemViewModel
{
    public string Name { get; set; } = "";
    public string RelativePath { get; set; } = "";
    public long SizeBytes { get; set; }
    public DateTime UploadedAt { get; set; }
    public string Extension { get; set; } = "";
    public string SizeDisplay =>
        SizeBytes < 1024 ? $"{SizeBytes} B" :
        SizeBytes < 1024 * 1024 ? $"{SizeBytes / 1024.0:F1} KB" :
        $"{SizeBytes / (1024.0 * 1024):F1} MB";
}
