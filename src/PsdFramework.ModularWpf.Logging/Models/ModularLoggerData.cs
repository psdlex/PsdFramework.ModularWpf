using Microsoft.Extensions.Logging;

namespace PsdFramework.ModularWpf.Logging.Models;

public sealed class ModularLoggerData
{
    public string? Path { get; init; }
    public string[] AllowedNamespaces { get; init; } = [];

    public LogLevel MinimumLevel { get; init; } = LogLevel.None;
    public LoggingSink Sink { get; init; } = LoggingSink.None;
}