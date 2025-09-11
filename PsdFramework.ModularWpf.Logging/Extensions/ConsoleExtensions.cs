namespace PsdFramework.ModularWpf.Logging.Extensions;

internal static class ConsoleExtensions
{
    public static void WriteColored(object value, ConsoleColor color)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(value);
        Console.ForegroundColor = prev;
    }

    public static void WriteColoredLine(object value, ConsoleColor color)
        => WriteColored(value + Environment.NewLine, color);
}