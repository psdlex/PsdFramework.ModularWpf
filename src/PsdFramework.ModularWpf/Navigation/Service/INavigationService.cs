using PsdFramework.ModularWpf.Interceptors;

namespace PsdFramework.ModularWpf.Navigation.Service;

public interface INavigationService : IInterceptableService<NavigationContext>
{
    public Task NavigateAsync(NavigationOptions options);
}
