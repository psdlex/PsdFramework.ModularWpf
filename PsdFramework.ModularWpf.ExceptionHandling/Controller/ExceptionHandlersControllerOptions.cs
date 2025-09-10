namespace PsdFramework.ModularWpf.ExceptionHandling.Controller;

public sealed class ExceptionHandlersControllerOptions
{
    public ControllerUnhandledExceptionBehavior UnhandledExceptionBehavior { get; init; } = ControllerUnhandledExceptionBehavior.Default;
}