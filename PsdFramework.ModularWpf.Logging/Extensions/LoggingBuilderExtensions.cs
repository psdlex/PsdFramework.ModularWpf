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
        
        foreach (var logger in loggers)
        {
            if (logger.Sink == LoggingSink.Console)
                builder.AddProvider(new ConsoleLoggerProvider(logger));

            else if (logger.Sink == LoggingSink.File)
                builder.AddProvider(new FileLoggerProvider(logger));
        }

        return builder;
    }

}