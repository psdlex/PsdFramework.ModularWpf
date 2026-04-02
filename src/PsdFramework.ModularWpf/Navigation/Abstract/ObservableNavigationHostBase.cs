using CommunityToolkit.Mvvm.ComponentModel;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;

namespace PsdFramework.ModularWpf.Navigation.Abstract;

public abstract class ObservableNavigationHostBase : ObservableObject, INavigationHost
{
    public INavigatable? CurrentModel { get; protected set => SetProperty(ref field, value); }

    public virtual Task OnNavigatingAsync(NavigationContext context) => Task.CompletedTask;
    public virtual Task OnNavigatedAsync(NavigationContext context)
    {
        CurrentModel = context.Navigatable;
        return Task.CompletedTask;
    }
}