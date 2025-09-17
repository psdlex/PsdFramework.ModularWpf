using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.Navigations.Service;

namespace PsdFramework.ModularWpf.Navigations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNavigator(this IServiceCollection services)
        => services.AddScoped<INavigatorService, NavigatorService>();
}