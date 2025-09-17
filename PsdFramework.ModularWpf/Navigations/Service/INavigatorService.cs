using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Service;

public interface INavigatorService : IComponentModel
{
    Task NavigateTo<TNavigatable>(INavigationComponentModel navigation)
        where TNavigatable : INavigatableComponentModel;

    Task NavigateTo<TNavigation, TNavigatable>()
        where TNavigation : INavigationComponentModel
        where TNavigatable : INavigatableComponentModel;

    Task NavigateTo<TNavigatable>(object category)
        where TNavigatable : INavigatableComponentModel;

    Task NavigateTo(Type navigatable, object category);
}