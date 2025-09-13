using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.Navigations.Models;

namespace PsdFramework.ModularWpf.Navigations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNavigationModels(this IServiceCollection services) => AddNavigationModels(services, _ => true);
    public static IServiceCollection AddNavigationModels(this IServiceCollection services, Func<Assembly, bool> assemblyFilter)
    {
        var pairs = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(assemblyFilter)
            .SelectMany(a => a.GetTypes())

            .Select(t => (Type: t, Attribute: t.GetCustomAttribute<NavigationModelAttribute>()!))
            .Where(t => t.Attribute is not null && t.Type.IsAssignableTo(typeof(INavigationModel)))

            .ToArray();

        foreach (var pair in pairs)
        {
            var descriptor = pair.Attribute.RegisterAsSeperateService
                ? new ServiceDescriptor(
                    typeof(INavigationModel),
                    pair.Attribute.NavigationModelId,
                    pair.Type,
                    ServiceLifetime.Scoped
                )
                : new ServiceDescriptor(
                    typeof(INavigationModel),
                    pair.Attribute.NavigationModelId,
                    (p, _) => p.GetRequiredService(pair.Type),
                    ServiceLifetime.Scoped
                );

            services.Add(descriptor);
        }

        return services;
    }
}