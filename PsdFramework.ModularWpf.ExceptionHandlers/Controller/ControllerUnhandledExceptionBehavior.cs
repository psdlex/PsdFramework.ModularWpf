namespace PsdFramework.ModularWpf.ExceptionHandlers.Controller;

public enum ControllerUnhandledExceptionBehavior : byte
{
    Ignore,
    Rethrow,
    Terminate,

    Default = Rethrow
}