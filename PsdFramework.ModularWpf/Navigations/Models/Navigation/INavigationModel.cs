using PsdFramework.ModularWpf.Navigations.Models.Navigatable;

namespace PsdFramework.ModularWpf.Navigations.Models.Navigation;

public interface INavigationModel
{
    Task NavigateToAsync<TModel>() where TModel : INavigatableModel;
    Task NavigateToAsync(Type type);
    Task NavigateToAsync(INavigatableModel model);
}