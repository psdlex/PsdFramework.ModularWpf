using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Navigation.Service;

namespace PsdFramework.ModularWpf.Navigation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNavigationService(this IServiceCollection services)
        => services.AddSingleton<INavigationService, NavigationService>();
}