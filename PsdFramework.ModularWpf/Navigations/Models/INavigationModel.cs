using CommunityToolkit.Mvvm.ComponentModel;

namespace PsdFramework.ModularWpf.Navigations.Models;

public interface INavigationModel
{
    Task NavigateToAsync<TModel>() where TModel : ObservableObject;
    Task NavigateToAsync(Type type);
    Task NavigateToAsync(ObservableObject model);
}