using PsdFramework.ModularWpf.Navigation.Navigatable;

namespace PsdFramework.ModularWpf.Navigation.Abstract;

public abstract partial class ObservableNavigationHostAndNavigatableBase : ObservableNavigationHostBase, INavigatable
{
    public virtual Task OnNavigatedFromAsync(NavigationContext context) => Task.CompletedTask;
    public virtual Task OnNavigatedToAsync(NavigationContext context) => Task.CompletedTask;
    public virtual Task OnNavigatingFromAsync(NavigationContext context) => Task.CompletedTask;
    public virtual Task OnNavigatingToAsync(NavigationContext context) => Task.CompletedTask;
}