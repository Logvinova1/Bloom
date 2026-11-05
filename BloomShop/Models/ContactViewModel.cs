namespace BloomShop.Models;

/// <summary>
/// Contact form model. Validation rules are in ContactViewModelValidator (FluentValidation).
/// </summary>
public class ContactViewModel
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Message { get; set; } = "";
}
