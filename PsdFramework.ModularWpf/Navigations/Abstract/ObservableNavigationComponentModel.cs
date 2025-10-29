using CommunityToolkit.Mvvm.ComponentModel;
using PsdFramework.ModularWpf.Navigations.Models;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Abstract;

public abstract partial class ObservableNavigationComponentModel : ObservableObject, INavigationComponentModel
{
    [ObservableProperty]
    private INavigatableComponentModel? _currentModel;

    public virtual Task OnNavigating(NavigationContext context)
    {
        return Task.CompletedTask;
    }
    public virtual Task OnNavigated(NavigationContext context)
    {
        CurrentModel = context.NavigatableComponentModel;
        return Task.CompletedTask;
    }
}