using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.General.Registration;
using PsdFramework.ModularWpf.Internal.FeatureUtiliser;
using PsdFramework.ModularWpf.PopupWindows.Models;

namespace PsdFramework.ModularWpf.PopupWindows;

internal sealed class FeatureUtiliser : IFeatureUtiliser
{
    public bool TryUtilise(IServiceCollection services, ComponentModelType type)
    {
        var modelInterface = type
            .ModelType
            .GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IPopupComponentModel<,>));

        if (modelInterface is null)
            return false;

        if (type.ModelType.IsAssignableTo(typeof(ObservableObject)) == false)
            throw new InvalidOperationException($"Component model must derive '{nameof(ObservableObject)}'");

        services.AddTransient(modelInterface, p =>
        {
            var model = p.GetRequiredKeyedService(typeof(IComponentModel), type.ModelType);
            return model;
        });

        return true;
    }
}