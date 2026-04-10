using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Internal;

namespace PsdFramework.ModularWpf.Navigation.NavigationHost;

internal sealed class NavigationHostComponentUtiliser : ComponentUtiliser<NavigationHostAttribute>
{
    public override void Utilise(IServiceCollection services, ComponentDescription description)
    {
        if (description.ModelType.GetInterfaces().Any(i => i == typeof(INavigationHost)) == false)
            ExceptionHelper.ThrowComponentModelInterfaceImplementationRequired(typeof(NavigationHostAttribute), typeof(INavigationHost));

        if (description.IsSharedModel)
        {
            services.AddKeyedSingleton(
                serviceType: typeof(INavigationHost),
                serviceKey: description.ModelType,
                implementationFactory: (p, k) => p.GetRequiredService((Type)k!)
            );
        }

        else
        {
            services.Add(new ServiceDescriptor(
                serviceType: typeof(INavigationHost),
                serviceKey: description.ModelType,
                implementationType: description.ModelType,
                lifetime: description.IsCached ? ServiceLifetime.Singleton : ServiceLifetime.Transient
            ));
        }

        if (((NavigationHostAttribute)description.Attribute).Category is { } category)
        {
            if (description.IsCached == false)
                throw new InvalidOperationException($"Navigation category requires caching the '{nameof(NavigationHost)}' component.");

            services.Add(new ServiceDescriptor(
                serviceType: typeof(INavigationHost),
                factory: (p, _) => p.GetRequiredKeyedService<INavigationHost>(description.ModelType),
                serviceKey: category,
                lifetime: ServiceLifetime.Singleton
            ));
        }
    }
}