using System.Windows.Threading;

namespace PsdFramework.ModularWpf.ModuarApplication;

public interface IModularApplication
{
    event DispatcherUnhandledExceptionEventHandler DispatcherUnhandledException;
    IServiceProvider? ServiceProvider { get; }
}