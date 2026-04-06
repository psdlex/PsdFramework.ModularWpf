using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Internal;
using System.Reflection;

namespace PsdFramework.ModularWpf.General;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddComponents(this IServiceCollection services) => AddComponents(services, _ => true);
    public static IServiceCollection AddComponents(this IServiceCollection services, Func<Assembly, bool> assemblyFilter, bool cacheByDefault = false)
    {
        var modelTypes = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(assemblyFilter)
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
            var isShared = ComponentUtility.IsModelShared(modelType);

            if (isShared)
                services.AddSingleton(modelType);

            foreach (var utiliser in utilisers)
            {
                if (ComponentUtility.TryCreateDescription(
                    modelType,
                    utiliser.AttributeType,
                    isShared: isShared,
                    isCachedByDefault: cacheByDefault,
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