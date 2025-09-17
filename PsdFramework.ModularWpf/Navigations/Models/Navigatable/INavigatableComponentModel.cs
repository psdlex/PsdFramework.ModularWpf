using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Models.Navigatable;

public interface INavigatableComponentModel : IComponentModel
{
    Task OnNavigated(INavigationComponentModel navigation);
}