using System.Runtime.ExceptionServices;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using PsdFramework.ModularWpf.ExceptionHandling.Models;

namespace PsdFramework.ModularWpf.ExceptionHandling.Controller;

public sealed class ExceptionHandlersController
{
    private readonly ExceptionHandlersControllerOptions _options;
    private readonly IExceptionHandler[] _handlers;
    private readonly ILogger<ExceptionHandlersController> _logger;

    public ExceptionHandlersController(
        IOptions<ExceptionHandlersControllerOptions> options,
        ILogger<ExceptionHandlersController> logger,
        IEnumerable<IExceptionHandler> handlers)
    {
        _options = options.Value;
        _logger = logger;

        var unsortedHandlers = handlers;
        _handlers = GetSortedHandlers(unsortedHandlers).ToArray();
    }

    public async Task Handle(DispatcherUnhandledExceptionEventArgs e)
    {
        _logger.LogWarning("Exception has been thrown and is being handled: {Exception}", e.Exception.Message);

        var context = new ModularContext(e);

        foreach (var handler in GetSortedHandlers(_handlers))
        {
            await handler.Handle(context);

            if (context.IsHandled)
            {
                _logger.LogInformation("Exception handled by {Handler}.", handler.GetType().Name);
                return;
            }
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