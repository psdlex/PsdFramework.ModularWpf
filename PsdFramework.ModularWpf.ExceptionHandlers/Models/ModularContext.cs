namespace PsdFramework.ModularWpf.ExceptionHandlers.Models;

public sealed class ModularContext
{
    public ModularContext(Exception exception)
    {
        Exception = exception;
    }

    public Exception Exception { get; }

    public bool IsHandled { get; set; }
}