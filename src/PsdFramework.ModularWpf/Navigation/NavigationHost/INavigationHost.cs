using PsdFramework.ModularWpf.Navigation.Navigatable;

namespace PsdFramework.ModularWpf.Navigation.NavigationHost;

public interface INavigationHost
{
    INavigatable? CurrentModel { get; }

    Task OnNavigatingAsync(NavigationContext context);
    Task OnNavigatedAsync(NavigationContext context);
}