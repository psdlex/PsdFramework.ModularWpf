using Microsoft.Extensions.Options;
using PsdFramework.ModularWpf.ExceptionHandling.Models;
using System.Runtime.ExceptionServices;
using System.Windows;
using System.Windows.Threading;

namespace PsdFramework.ModularWpf.ExceptionHandling.Controller;

public sealed class ExceptionHandlersController
{
    private readonly ExceptionHandlersControllerOptions _options;
    private readonly IExceptionHandler[] _handlers;

    public ExceptionHandlersController(
        IOptions<ExceptionHandlersControllerOptions> options,
        IEnumerable<IExceptionHandler> handlers)
    {
        _options = options.Value;

        var unsortedHandlers = handlers;
        _handlers = GetSortedHandlers(unsortedHandlers).ToArray();
    }

    public async Task Handle(DispatcherUnhandledExceptionEventArgs e)
    {
        var context = new ModularContext(e);

        foreach (var handler in GetSortedHandlers(_handlers))
        {
            await handler.Handle(context);

            if (context.IsHandled)
                return;
        }

        ProcessUnhandledException(e.Exception);
    }

    private void ProcessUnhandledException(Exception exception)
    {
        switch (_options.UnhandledExceptionBehavior)
        {
            case ControllerUnhandledExceptionBehavior.Ignore:
                return;
            case ControllerUnhandledExceptionBehavior.Rethrow:
                ExceptionDispatchInfo
                    .Capture(exception)
                    .Throw();
                break;

            case ControllerUnhandledExceptionBehavior.Terminate:
                Application.Current.Shutdown(1);
                break;
        }
    }

    private static IEnumerable<IExceptionHandler> GetSortedHandlers(IEnumerable<IExceptionHandler> handlers)
    {
        return handlers
            .OrderBy(h =>
            {
                var type = h.GetType();

                return type.IsGenericType
                    ? type.GetGenericArguments()[0] == typeof(Exception) ? 1 : 0
                    : 1;
            });
    }
}