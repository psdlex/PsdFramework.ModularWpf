using System.Reflection;

namespace PsdFramework.ModularWpf.Logging.Loggers;

public static class VarMaps
{
    private static readonly IReadOnlyDictionary<string, Func<string>> _maps = new Dictionary<string, Func<string>>(StringComparer.OrdinalIgnoreCase)
    {
        { "$RunningProjectName", () => Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty }
    };

    public static string? GetVarMap(string varMap)
        => _maps.TryGetValue(varMap, out var map) ? map() : null;
}