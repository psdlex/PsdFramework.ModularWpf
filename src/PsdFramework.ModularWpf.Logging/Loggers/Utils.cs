using Microsoft.Extensions.Logging;

namespace PsdFramework.ModularWpf.Logging.Loggers;

internal static class Utils
{
    public static (string Time, string LogLevel, ConsoleColor LogLevelColor, string Message) BuildGenericLog<TState>(LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var time = DateTime.Now.ToString("HH:mm:ss");
        var displayableLogLevel = ToDisplayableLog(logLevel);
        var message = formatter(state, exception);
        return (time, displayableLogLevel.Name, displayableLogLevel.Color, message);
    }

    private static (string Name, ConsoleColor Color) ToDisplayableLog(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => ("TRC", ConsoleColor.DarkCyan),
        LogLevel.Debug => ("DBG", ConsoleColor.Gray),
        LogLevel.Information => ("INF", ConsoleColor.DarkBlue),
        LogLevel.Warning => ("WRN", ConsoleColor.DarkYellow),
        LogLevel.Error => ("ERR", ConsoleColor.Red),
        LogLevel.Critical => ("CRT", ConsoleColor.DarkRed),

        _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
    };
}