using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace BloomShop.Localization;

public class JsonStringLocalizer : IStringLocalizer
{
    private static readonly ConcurrentDictionary<string, Dictionary<string, string>> Cache = new();
    private readonly string _resourcesPath;

    public JsonStringLocalizer(string resourcesPath) => _resourcesPath = resourcesPath;

    private Dictionary<string, string> Load()
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        return Cache.GetOrAdd(culture, c =>
        {
            var path = Path.Combine(_resourcesPath, $"{c}.json");
            if (!File.Exists(path)) path = Path.Combine(_resourcesPath, "en.json");
            if (!File.Exists(path)) return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
                return dict ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
            catch { return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); }
        });
    }

    public LocalizedString this[string name]
    {
        get
        {
            var dict = Load();
            return dict.TryGetValue(name, out var v) && !string.IsNullOrEmpty(v)
                ? new LocalizedString(name, v, false)
                : new LocalizedString(name, name, true);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var s = this[name];
            return new LocalizedString(name, string.Format(s.Value, arguments), s.ResourceNotFound);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
        Load().Select(kv => new LocalizedString(kv.Key, kv.Value, false));
}

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly string _resourcesPath;
    public JsonStringLocalizerFactory(IWebHostEnvironment env)
        => _resourcesPath = Path.Combine(env.ContentRootPath, "Resources");

    public IStringLocalizer Create(Type resourceSource) => new JsonStringLocalizer(_resourcesPath);
    public IStringLocalizer Create(string baseName, string location) => new JsonStringLocalizer(_resourcesPath);
}
