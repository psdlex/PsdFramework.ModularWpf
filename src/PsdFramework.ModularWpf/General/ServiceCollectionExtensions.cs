using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PsdFramework.ModularWpf.Internal;
using PsdFramework.ModularWpf.Navigation.Service;
using PsdFramework.ModularWpf.Popup.Service;

namespace PsdFramework.ModularWpf.General;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddComponents(this IServiceCollection services) => AddComponents(services, ComponentOptions.Empty());
    public static IServiceCollection AddComponents(this IServiceCollection services, ComponentOptions options)
    {
        services.TryAddTransient<IPopupService, PopupService>();
        services.TryAddTransient<INavigationService, NavigationService>();

        var modelTypes = options.ConcreteTypes?.ToArray() ?? AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(options.AssemblyFilter ?? (_ => true))
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                t.IsClass &&
                t.IsAbstract == false &&
                t.IsDefined(typeof(ComponentAttribute), inherit: true)
            )
            .ToArray();

        var utilisers = ComponentUtilisersLocator.LocateAll();

        foreach (var modelType in modelTypes)
        {
            var utilised = false;
            var isShared = ComponentUtility.IsSharedComponentModel(modelType);

            if (isShared)
                services.AddSingleton(modelType);

            foreach (var utiliser in utilisers)
            {
                if (ComponentUtility.TryCreateDescription(
                    modelType,
                    utiliser.AttributeType,
                    isShared: isShared,
                    isCachedByDefault: options.CacheByDefault,
                    out var description) == false)
                {
                    continue;
                }

                utiliser.Utilise(services, description);
                utilised = true;
            }

            if (utilised == false)
                throw new InvalidOperationException($"Can't utilise any components for model '{modelType}'.");
        }

        return services;
    }
}