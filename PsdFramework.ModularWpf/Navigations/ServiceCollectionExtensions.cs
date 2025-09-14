using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;
using PsdFramework.ModularWpf.Navigations.Provider;

namespace PsdFramework.ModularWpf.Navigations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNavigations(this IServiceCollection services) => AddNavigations(services, _ => true);
    public static IServiceCollection AddNavigations(this IServiceCollection services, Func<Assembly, bool> assemblyFilter)
    {
        var types = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(assemblyFilter)
            .SelectMany(a => a.GetTypes())
            .ToArray();

        AddNavigationModels(services, types);
        AddNavigatableModels(services, types);

        services.AddScoped<INavigationProvider, NavigationProvider>();

        return services;
    }

    private static void AddNavigationModels(IServiceCollection services, Type[] types)
    {
        var modelTypes = types
            .Select(t => (Type: t, Attribute: t.GetCustomAttribute<NavigationModelAttribute>()!))
            .Where(t =>
                t.Attribute is not null &&
                t.Type.IsClass &&
                t.Type.IsAbstract == false &&
                t.Type.IsAssignableTo(typeof(INavigationModel))
            ).ToArray();

        if (modelTypes.Length == 0)
            throw new InvalidOperationException("PsdFramework.ModularWpf could not find a single NAVIGATION model to register.");

        foreach (var type in modelTypes)
        {
            services.AddKeyedScoped(
                typeof(INavigationModel),
                type.Attribute.Category,
                type.Type
            );
        }
    }

    private static void AddNavigatableModels(IServiceCollection services, Type[] types)
    {
        var modelTypes = types
            .Select(t => (Type: t, Attribute: t.GetCustomAttribute<NavigatableModelAttribute>()!))
            .Where(t =>
                t.Attribute is not null &&
                t.Type.IsClass &&
                t.Type.IsAbstract == false &&
                t.Type.IsAssignableTo(typeof(INavigatableModel))
            ).ToArray();

        if (modelTypes.Length == 0)
            throw new InvalidOperationException("PsdFramework.ModularWpf could not find a single NAVIGATABLE model to register.");

        foreach (var type in modelTypes)
        {
            if (type.Attribute.Categories.Any() == false)
                throw new ArgumentException("NavigatableModels must have at least 1 category.");

            var isCached = type
                .Type
                .GetInterfaces()
                .Any(i => i == typeof(ICachedNavigatableModel));

            Func<Type, object, Type, IServiceCollection> func = isCached
                ? services.AddKeyedScoped
                : services.AddKeyedTransient;

            foreach (var category in type.Attribute.Categories)
            {
                func(type.Type, category, type.Type);
                func(typeof(INavigatableModel), category, type.Type);
            }
        }
    }
}