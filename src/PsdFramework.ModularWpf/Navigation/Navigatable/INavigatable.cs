namespace PsdFramework.ModularWpf.Navigation.Navigatable;

public interface INavigatable
{
    Task OnNavigatingToAsync(NavigationContext context);
    Task OnNavigatedToAsync(NavigationContext context);
    Task OnNavigatingFromAsync(NavigationContext context);
    Task OnNavigatedFromAsync(NavigationContext context);
}