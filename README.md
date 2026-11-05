# Bloom — women's clothing shop (ASP.NET Core 8 MVC)

Small boutique site covering all lab topics:

1. **MVC + Bootstrap 5** (LibMan) + custom elegant UI  
2. **Email** — `BloomShop.Services` / MailKit / Contact form / Scoped DI  
3. **Request logging** — `Logs/requests-yyyy-MM-dd.log` (full URL, time, IP) + `.gitignore`  
4. **File upload** — `wwwroot/Files`, list + preview  
5. **Localization EN/UA** — shared JSON resources, `SetLanguage`, `_LanguageSwitcher`

## Run

```bash
cd BloomShop
dotnet restore
# optional: libman restore   (or use CDN paths already in layout)
dotnet run --project BloomShop
```

Open https://localhost:7201

## Email

Edit `BloomShop/appsettings.json` → `EmailSettings` (Gmail App Password).

## Labs checklist

| Lab | Where |
|-----|--------|
| Services DLL | `BloomShop.Services` |
| IEmailSender | MailKit SMTP |
| Logs + gitignore | `Middleware/RequestLoggingMiddleware`, `.gitignore` |
| Upload | `/Files` |
| i18n | EN/UA buttons, `Resources/en.json` + `uk.json` |
