using System.Windows.Threading;

using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.ExceptionHandling.Controller;
using PsdFramework.ModularWpf.General.Models;

namespace PsdFramework.ModularWpf.ExceptionHandling.Extensions;

public static class ModularApplicationExtensions
{
    public static IModularApplication AddGlobalExceptionHandler(this IModularApplication app)
    {
        DispatcherUnhandledExceptionEventHandler handler = null!;
        handler = async (sender, e) =>
        {
            try
            {
                var controller = app.ServiceProvider?.GetRequiredService<ExceptionHandlersController>()
                    ?? throw new ApplicationException("ServiceProvider is not configured or ExceptionHandlersController is not registered.");

                await controller.Handle(e);
            }
            catch
            {
                app.DispatcherUnhandledException -= handler;
                throw; // let it propagate normally
            }
        };

        app.DispatcherUnhandledException += handler;
        return app;
    }
}