using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Internal;
using PsdFramework.ModularWpf.View.Models;
using System.Windows;

namespace PsdFramework.ModularWpf.View;

internal sealed class ViewComponentUtiliser : ComponentUtiliser<ViewAttribute>
{
    public override void Utilise(IServiceCollection services, ComponentDescription description)
    {
        var viewType = ((ViewAttribute)description.Attribute).ViewType;

        var noModelInterfaceType = typeof(IView<>).MakeGenericType(viewType);
        var interfaceType = typeof(IView<,>).MakeGenericType(viewType, description.ModelType);
        var implementationType = typeof(ViewRepresentation<,>).MakeGenericType(viewType, description.ModelType);

        if (description.IsSharedModel == false)
        {
            services.Add(new ServiceDescriptor(
                serviceType: description.ModelType,
                implementationType: description.ModelType,
                lifetime: description.IsCached ? ServiceLifetime.Singleton : ServiceLifetime.Transient
            ));
        }

        services.Add(new ServiceDescriptor(
            serviceType: viewType,
            factory: (_) => Activator.CreateInstance(viewType)!,
            lifetime: description.IsSharedModel || description.IsCached ? ServiceLifetime.Singleton : ServiceLifetime.Transient
        ));

        services.Add(new ServiceDescriptor(
            serviceType: interfaceType,
            factory: (p) =>
            {
                var bindedView = CreateBindedView(p, description.ModelType, viewType);
                return Activator.CreateInstance(implementationType, bindedView.View, bindedView.Model)!;
            },
            lifetime: description.IsSharedModel || description.IsCached ? ServiceLifetime.Singleton : ServiceLifetime.Transient
        ));

        services.Add(new ServiceDescriptor(
            serviceType: noModelInterfaceType,
            factory: (p) =>
            {
                return p.GetRequiredService(interfaceType);
            },
            lifetime: description.IsSharedModel || description.IsCached ? ServiceLifetime.Singleton : ServiceLifetime.Transient
        ));
    }

    private static (object Model, FrameworkElement View) CreateBindedView(IServiceProvider provider, Type modelType, Type viewType)
    {
        var model = provider.GetRequiredService(modelType);
        var view = (FrameworkElement)provider.GetRequiredService(viewType);
        view.DataContext = model;

        return (model, view);
    }

}