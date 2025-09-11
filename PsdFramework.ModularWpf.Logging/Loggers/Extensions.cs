using Microsoft.Extensions.Logging;

namespace PsdFramework.ModularWpf.Logging.Loggers;

internal static class Extensions
{
    public static bool HasAllowedNamespace(this IModularLogger logger, string categoryName)
    {
        var namespaces = logger
            .Data
            .AllowedNamespaces
            .Select(x => x[0] == '$' ? VarMaps.GetVarMap(x) : x)
            .ToArray();

        return namespaces.Length == 0
        || namespaces.Any(ns => 
            ns is not null &&
            categoryName.StartsWith(ns, StringComparison.OrdinalIgnoreCase)
        );
    }

    public static bool HasAcceptableLogLevel(this IModularLogger logger, LogLevel logLevel)
        => logger.Data.MinimumLevel == LogLevel.None 
        || logger.Data.MinimumLevel <= logLevel;
}