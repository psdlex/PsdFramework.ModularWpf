namespace PsdFramework.ModularWpf.Navigation.Service;

public interface INavigationService
{
    public Task NavigateAsync(NavigationOptions options);
}
