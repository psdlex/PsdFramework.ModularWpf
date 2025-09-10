namespace PsdFramework.ModularWpf.ExceptionHandlers.Controller;

public sealed class ExceptionHandlersControllerOptions
{
    public ControllerUnhandledExceptionBehavior UnhandledExceptionBehavior { get; init; } = ControllerUnhandledExceptionBehavior.Default;
}