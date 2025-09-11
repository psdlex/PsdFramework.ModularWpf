using Microsoft.Extensions.Logging;

using PsdFramework.ModularWpf.Logging.Extensions;
using PsdFramework.ModularWpf.Logging.Models;

namespace PsdFramework.ModularWpf.Logging.Loggers.ConsoleLogger;

public sealed class ConsoleLogger : ILogger, IModularLogger
{
    private readonly string _categoryName;

    public ConsoleLogger(ModularLoggerData modularLogger, string categoryName)
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

        var log = Utils.BuildGenericLog(logLevel, state, exception, formatter);

        ConsoleExtensions.WriteColored($"[{log.Time}] ", ConsoleColor.DarkGray);
        ConsoleExtensions.WriteColored($"[{log.LogLevel}] ", log.LogLevelColor);
        Console.WriteLine(log.Message);
    }
}