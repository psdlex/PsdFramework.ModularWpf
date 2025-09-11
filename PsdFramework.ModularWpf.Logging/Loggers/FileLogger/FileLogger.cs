using System.IO;

using Microsoft.Extensions.Logging;

using PsdFramework.ModularWpf.Logging.Models;

namespace PsdFramework.ModularWpf.Logging.Loggers.FileLogger;

public sealed class FileLogger : ILogger, IModularLogger
{
    private readonly string _categoryName;

#if NET9_0_OR_GREATER
    private readonly Lock _lock = new();
#else
    private static readonly object _lock = new();
#endif

    public FileLogger(ModularLoggerData modularLogger, string categoryName)
    {
        Data = modularLogger;
        _categoryName = categoryName;
    }

    public ModularLoggerData Data { get; }

    public IDisposable BeginScope<TState>(TState state) => null!;
    public bool IsEnabled(LogLevel logLevel)
        => Data.MinimumLevel == LogLevel.None
        || Data.MinimumLevel <= logLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (this.HasAllowedNamespace(_categoryName) == false || this.HasAcceptableLogLevel(logLevel) == false)
            return;

        if (Data.Path is null)
            throw new InvalidOperationException("Path is not set.");

        var log = Utils.BuildGenericLog(logLevel, state, exception, formatter);

        lock (_lock)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), Path.GetDirectoryName(Data.Path) ?? "");

            Directory.CreateDirectory(dir);

            Action<string, string> action = File.Exists(Data.Path) ? File.AppendAllText : File.WriteAllText;
            action(Data.Path, $"[{log.Time}] [{log.LogLevel}] {log.Message}{Environment.NewLine}");
        }
    }
}