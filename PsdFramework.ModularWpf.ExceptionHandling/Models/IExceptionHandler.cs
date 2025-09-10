namespace PsdFramework.ModularWpf.ExceptionHandling.Models;

public interface IExceptionHandler
{
    Task Handle(ModularContext context);
}

public interface IExceptionHandler<TExpectedException> : IExceptionHandler
    where TExpectedException : Exception;