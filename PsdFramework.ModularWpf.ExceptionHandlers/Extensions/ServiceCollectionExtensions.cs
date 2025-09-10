using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.ExceptionHandlers.Controller;
using PsdFramework.ModularWpf.ExceptionHandlers.Models;

namespace PsdFramework.ModularWpf.ExceptionHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExceptionHandlersController(this IServiceCollection services) => services.AddExceptionHandlersController(_ => { });
    public static IServiceCollection AddExceptionHandlersController(this IServiceCollection services, Action<ExceptionHandlersControllerOptions> configuration)
    {
        services.AddSingleton<ExceptionHandlersController>();
        services.Configure(configuration);

        return services;
    }

    public static IServiceCollection AddExceptionHandler<THandler>(this IServiceCollection services)
        where THandler : class, IExceptionHandler
    {
        services.AddTransient<IExceptionHandler, THandler>();
        return services;
    }
}