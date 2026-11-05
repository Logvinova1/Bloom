using System.Text;

namespace BloomShop.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private static readonly object Lock = new();

    public RequestLoggingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var req = context.Request;
        var fullUrl = $"{req.Scheme}://{req.Host}{req.PathBase}{req.Path}{req.QueryString}";
        var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var fwd))
            ip = fwd.ToString().Split(',')[0].Trim();

        var line = $"[{time}] URL: {fullUrl} | IP: {ip}{Environment.NewLine}";
        var dir = Path.Combine(_env.ContentRootPath, "Logs");
        Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, $"requests-{DateTime.Now:yyyy-MM-dd}.log");
        lock (Lock) { File.AppendAllText(file, line, Encoding.UTF8); }

        await _next(context);
    }
}

public static class RequestLoggingExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        => app.UseMiddleware<RequestLoggingMiddleware>();
}
