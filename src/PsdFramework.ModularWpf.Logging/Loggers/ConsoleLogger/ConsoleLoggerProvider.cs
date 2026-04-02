using Microsoft.Extensions.Logging;

using PsdFramework.ModularWpf.Logging.Models;

namespace PsdFramework.ModularWpf.Logging.Loggers.ConsoleLogger;

public sealed class ConsoleLoggerProvider : ILoggerProvider
{
    private readonly ModularLoggerData _modularLogger;

    public ConsoleLoggerProvider(ModularLoggerData modularLogger)
    {
        _modularLogger = modularLogger;
    }

    public ILogger CreateLogger(string categoryName) => new ConsoleLogger(_modularLogger, categoryName);

    public void Dispose() { }
}