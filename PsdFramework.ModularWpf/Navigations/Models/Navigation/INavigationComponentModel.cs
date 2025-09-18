using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;

namespace PsdFramework.ModularWpf.Navigations.Models.Navigation;

public interface INavigationComponentModel : IComponentModel
{
    INavigatableComponentModel? CurrentModel { get; }
    Task OnPreNavigated(INavigatableComponentModel navigatable);
    Task OnNavigated(INavigatableComponentModel navigatable);
}