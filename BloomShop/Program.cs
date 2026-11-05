using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using BloomShop.Localization;
using BloomShop.Middleware;
using BloomShop.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using BloomShop.Validators;

var builder = WebApplication.CreateBuilder(args);

var logsPath = Path.Combine(builder.Environment.ContentRootPath, "Logs");
Directory.CreateDirectory(logsPath);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddProvider(new BloomShop.Logging.FileLoggerProvider(Path.Combine(logsPath, "app-{date}.log")));

builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
builder.Services.AddLocalization();

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// FluentValidation (labs 7–8)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ContactViewModelValidator>();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supported = new[] { new CultureInfo("en"), new CultureInfo("uk") };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supported;
    options.SupportedUICultures = supported;
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
// Scoped — один екземпляр на HTTP-запит (також є Transient / Singleton)
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();
app.UseRequestLogging();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
