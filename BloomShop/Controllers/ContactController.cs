using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BloomShop.Models;
using BloomShop.Services;

namespace BloomShop.Controllers;

public class ContactController : Controller
{
    private readonly IEmailSender _email;
    private readonly EmailSettings _settings;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IEmailSender email, IOptions<EmailSettings> opt, ILogger<ContactController> logger)
    {
        _email = email;
        _settings = opt.Value;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() => View(new ContactViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            var html = $"""
                <div style="font-family:Georgia,serif;max-width:560px;margin:auto;padding:24px;background:#fff8f5;color:#333;">
                  <h2 style="color:#c45c74;">Bloom Shop — new message</h2>
                  <p><b>From:</b> {System.Net.WebUtility.HtmlEncode(model.Name)}</p>
                  <p><b>Email:</b> {System.Net.WebUtility.HtmlEncode(model.Email)}</p>
                  <hr/>
                  <p style="white-space:pre-wrap;">{System.Net.WebUtility.HtmlEncode(model.Message)}</p>
                </div>
                """;
            await _email.SendEmailAsync(_settings.AdminEmail, $"[Bloom] Message from {model.Name}", html);
            await _email.SendEmailAsync(model.Email, "Bloom Shop — we received your message",
                $"<p>Hi {System.Net.WebUtility.HtmlEncode(model.Name)}, thanks for writing to Bloom. We will reply soon.</p>");
            TempData["Success"] = "OK";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email failed");
            ModelState.AddModelError("", $"Could not send email: {ex.InnerException?.Message ?? ex.Message}");
            return View(model);
        }
    }
}
