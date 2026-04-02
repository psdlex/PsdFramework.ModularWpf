using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Internal;
using PsdFramework.ModularWpf.Popup.Models;

namespace PsdFramework.ModularWpf.Popup;

internal sealed class PopupComponentUtiliser : ComponentUtiliser<PopupAttribute>
{
    public override void Utilise(IServiceCollection services, ComponentDescription description)
    {
        if (description.ModelType
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPopup<,>)) is not { } @interface)
        {
            return;
        }

        if (description.IsSharedModel)
        {
            services.AddKeyedSingleton(
                serviceType: @interface,
                serviceKey: description.ModelType,
                implementationFactory: (p, _) => p.GetRequiredService(description.ModelType)
            );
        }

        else
        {
            services.Add(new ServiceDescriptor(
                serviceType: @interface,
                serviceKey: description.ModelType,
                implementationType: description.ModelType,
                lifetime: description.IsCached ? ServiceLifetime.Singleton : ServiceLifetime.Transient
            ));
        }
    }
}