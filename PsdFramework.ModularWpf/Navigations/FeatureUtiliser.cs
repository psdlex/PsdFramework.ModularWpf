using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.General.Registration;
using PsdFramework.ModularWpf.Internal.FeatureUtiliser;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations;

internal sealed class FeatureUtiliser : IFeatureUtiliser
{
    public bool TryUtilise(IServiceCollection services, ComponentModelType type)
    {
        var utilised = false;

        if (type
            .ModelType
            .GetInterfaces()
            .Any(i => i == typeof(INavigationComponentModel)))
        {
            UtilizeNavigationModel(services, type);
            utilised = true;
        }

        if (type
            .ModelType
            .GetInterfaces()
            .Any(i => i == typeof(INavigatableComponentModel)))
        {
            UtilizeNavigatableModel(services, type);
            utilised = true;
        }

        return utilised;
    }

    private static void UtilizeNavigationModel(IServiceCollection services, ComponentModelType type)
    {
        var category = type.ModelType.GetCustomAttribute<NavigationComponentModelAttribute>()?.Category 
            ?? throw new InvalidOperationException($"'{nameof(INavigationComponentModel)}' must have category specified in an attribute '{nameof(NavigationComponentModelAttribute)}'.");

        services.AddKeyedTransient(
            typeof(INavigationComponentModel),
            type.ModelType,
            (p, _) => p.GetRequiredKeyedService<IComponentModel>(type.ModelType)
        );

        services.AddKeyedTransient(
            typeof(INavigationComponentModel),
            category,
            (p, _) => p.GetRequiredKeyedService<IComponentModel>(type.ModelType)
        );
    }

    private static void UtilizeNavigatableModel(IServiceCollection services, ComponentModelType type)
    {
        var categories = type
            .ModelType
            .GetCustomAttribute<NavigatableComponentModelAttribute>()?
            .Categories ?? throw new InvalidOperationException($"'{nameof(INavigatableComponentModel)}' must have category specified in an attribute '{nameof(NavigatableComponentModelAttribute)}'.");

        if (categories.Length == 0)
            throw new InvalidOperationException($"'{nameof(NavigatableComponentModelAttribute)}' must have at least 1 category.");

        foreach (var category in categories)
        {
            services.AddKeyedTransient(
                typeof(INavigatableComponentModel),
                category,
                (p, _) => p.GetRequiredKeyedService<IComponentModel>(type.ModelType)
            );

            services.AddKeyedTransient(
                typeof(INavigatableComponentModel),
                type.ModelType,
                (p, _) => p.GetRequiredKeyedService<IComponentModel>(type.ModelType)
            );
        }
    }
}