using Microsoft.Extensions.Logging;

using PsdFramework.ModularWpf.Logging.Models;

namespace PsdFramework.ModularWpf.Logging.Loggers.FileLogger;

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly ModularLoggerData _modularLogger;

    public FileLoggerProvider(ModularLoggerData modularLogger)
    {
        _modularLogger = modularLogger;
    }

    public ILogger CreateLogger(string categoryName) => new FileLogger(_modularLogger, categoryName);

    public void Dispose() { }
}