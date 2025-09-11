using Microsoft.Extensions.Logging;

namespace PsdFramework.ModularWpf.Logging.Loggers;

internal static class Extensions
{
    public static bool HasAllowedNamespace(this IModularLogger logger, string categoryName)
        => logger.Data.AllowedNamespaces.Length == 0
        || logger.Data.AllowedNamespaces.Any(ns => categoryName.StartsWith(ns, StringComparison.OrdinalIgnoreCase));

    public static bool HasAcceptableLogLevel(this IModularLogger logger, LogLevel logLevel)
        => logger.Data.MinimumLevel == LogLevel.None 
        || logger.Data.MinimumLevel <= logLevel;
}