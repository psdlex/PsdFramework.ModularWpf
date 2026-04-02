using PsdFramework.ModularWpf.Logging.Models;

namespace PsdFramework.ModularWpf.Logging.Loggers;

public interface IModularLogger
{
    ModularLoggerData Data { get; }
}