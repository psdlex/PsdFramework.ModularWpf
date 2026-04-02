using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Internal;
using PsdFramework.ModularWpf.Navigation.Navigatable;

namespace PsdFramework.ModularWpf.Navigation.ComponentsUtilisers;

internal sealed class NavigatableComponentUtiliser : ComponentUtiliser<NavigatableAttribute>
{
    public override void Utilise(IServiceCollection services, ComponentDescription description)
    {
        if (description.ModelType.GetInterfaces().Any(i => i == typeof(INavigatable)) == false)
            ExceptionHelper.ThrowComponentModelInterfaceImplementationRequired(typeof(NavigatableAttribute), typeof(INavigatable));

        if (description.IsSharedModel)
        {
            services.AddKeyedSingleton(
                serviceType: typeof(INavigatable),
                serviceKey: description.ModelType,
                implementationFactory: (p, k) => p.GetRequiredService((Type)k!)
            );
        }

        else
        {
            services.Add(new ServiceDescriptor(
                serviceType: typeof(INavigatable),
                serviceKey: description.ModelType,
                implementationType: description.ModelType,
                lifetime: description.IsCached ? ServiceLifetime.Singleton : ServiceLifetime.Transient
            ));
        }
    }
}
