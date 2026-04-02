using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using PsdFramework.ModularWpf.Logging.Loggers.ConsoleLogger;
using PsdFramework.ModularWpf.Logging.Loggers.FileLogger;
using PsdFramework.ModularWpf.Logging.Models;

namespace PsdFramework.ModularWpf.Logging.Extensions;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddPsdFramework(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var loggers = configuration.GetRequiredSection("ModularLoggers").Get<ModularLoggerData[]>();
        
        foreach (var logger in loggers ?? [])
        {
            Func<ModularLoggerData, ILoggerProvider> provider = logger.Sink switch
            {
                LoggingSink.Console => (data) => new ConsoleLoggerProvider(data),
                LoggingSink.File => (data) => new FileLoggerProvider(data),
                _ => throw new ArgumentException("Invalid LoggingSink", nameof(logger.Sink))
            };

            builder.AddProvider(provider(logger));
        }

        return builder;
    }

}