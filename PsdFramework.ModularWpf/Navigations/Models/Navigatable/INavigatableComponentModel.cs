using PsdFramework.ModularWpf.General.Models.Components;

namespace PsdFramework.ModularWpf.Navigations.Models.Navigatable;

public interface INavigatableComponentModel : IComponentModel
{
    Task OnNavigatedTo(NavigationContext context);
    Task OnNavigatingFrom(NavigationContext context);
}