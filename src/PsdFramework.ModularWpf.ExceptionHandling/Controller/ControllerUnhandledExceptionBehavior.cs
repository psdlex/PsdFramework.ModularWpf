namespace PsdFramework.ModularWpf.ExceptionHandling.Controller;

public enum ControllerUnhandledExceptionBehavior : byte
{
    Ignore,
    Rethrow,
    Terminate,

    Default = Rethrow
}