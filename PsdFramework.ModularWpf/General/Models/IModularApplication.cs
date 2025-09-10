using System.Windows.Threading;

namespace PsdFramework.ModularWpf.General.Models;

public interface IModularApplication
{
    event DispatcherUnhandledExceptionEventHandler DispatcherUnhandledException;

    IServiceProvider? ServiceProvider { get; }
}