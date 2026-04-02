using System.Windows.Threading;

namespace PsdFramework.ModularWpf.ExceptionHandling.Models;

public sealed class ModularContext
{
    private readonly DispatcherUnhandledExceptionEventArgs _exceptionArgs;

    public ModularContext(DispatcherUnhandledExceptionEventArgs exceptionArgs)
    {
        _exceptionArgs = exceptionArgs;
    }

    public Exception Exception => _exceptionArgs.Exception;
    public bool IsHandled
    {
        get => _exceptionArgs.Handled;
        set => _exceptionArgs.Handled = value;
    }
}