using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.General.Registration;
using PsdFramework.ModularWpf.Internal.FeatureUtiliser;
using PsdFramework.ModularWpf.Views.Models;
using PsdFramework.ModularWpf.Views.Models.Representation;

namespace PsdFramework.ModularWpf.Views;

internal sealed class FeatureUtiliser : IFeatureUtiliser
{
    public bool TryUtilise(IServiceCollection services, ComponentModelType type)
    {
        var viewInterface = type
            .ModelType
            .GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IViewComponentModel<>)
            );

        if (viewInterface is null)
            return false;

        if (type.ModelType.IsAssignableTo(typeof(ObservableObject)) == false)
            throw new InvalidOperationException($"Component model must derive '{nameof(ObservableObject)}'");

        var viewType = viewInterface
            .GetGenericArguments()
            .First();

        var representationInterfaceType = typeof(IView<,>).MakeGenericType(viewType, type.ModelType);
        var representationType = typeof(ViewRepresentation<,>).MakeGenericType(viewType, type.ModelType);
        var representationInterfaceTypeWithoutModel = typeof(IView<>).MakeGenericType(viewType);
        var representationTypeWithoutModel = typeof(ViewRepresentation<>).MakeGenericType(viewType);

        services.AddTransient(representationInterfaceType, p =>
        {
            var model = p.GetRequiredKeyedService<IComponentModel>(type.ModelType);
            var view = (Window)Activator.CreateInstance(viewType)!;
            view.DataContext = model;

            return Activator.CreateInstance(representationType, args: [view, model])!;
        });

        services.AddTransient(representationInterfaceTypeWithoutModel , p =>
        {
            var model = p.GetRequiredKeyedService<IComponentModel>(type.ModelType);
            var view = (Window)Activator.CreateInstance(viewType)!;
            view.DataContext = model;

            return Activator.CreateInstance(representationTypeWithoutModel, args: [view])!;
        });

        return true;
    }
}