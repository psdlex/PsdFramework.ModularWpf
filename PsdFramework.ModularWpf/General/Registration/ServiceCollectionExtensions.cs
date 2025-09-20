using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.Internal.FeatureUtiliser;

namespace PsdFramework.ModularWpf.General.Registration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddComponentModels(this IServiceCollection services) => services.AddComponentModels(_ => true);
    public static IServiceCollection AddComponentModels(this IServiceCollection services, Func<Assembly, bool> assemblyFilter)
    {
        var componentModelTypes = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(assemblyFilter)
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                t.IsClass &&
                t.IsAbstract == false &&
                t.IsAssignableTo(typeof(IComponentModel))
            )
            .Select(t => new ComponentModelType(t, t.GetCustomAttribute<ComponentModelAttribute>(inherit: true)?.IsCached ?? false))
            .ToArray();

        var utilisers = FeatureUtilisersLocator.FindAll();

        foreach (var type in componentModelTypes)
        {
            var descriptor = new ServiceDescriptor(
                typeof(IComponentModel),
                serviceKey: type.ModelType,
                type.ModelType,
                type.IsCached
                    ? ServiceLifetime.Scoped 
                    : ServiceLifetime.Transient
            );

            services.Add(descriptor);

            var utilised = false;
            foreach (var utiliser in utilisers)
                utilised = utiliser.TryUtilise(services, type) || utilised;

            if (utilised == false)
                throw new InvalidOperationException("Cant utilise that component model.");
        }

        return services;
    }
}