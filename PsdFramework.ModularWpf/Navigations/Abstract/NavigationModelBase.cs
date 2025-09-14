
using CommunityToolkit.Mvvm.ComponentModel;

using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;
using PsdFramework.ModularWpf.Navigations.Provider;

namespace PsdFramework.ModularWpf.Navigations.Abstract;

public abstract partial class NavigationModelBase : ObservableObject, INavigationModel
{
    protected NavigationModelBase(INavigationProvider navigationProvider, object category)
    {
        NavigationProvider = navigationProvider;
        Category = category;
    }

    protected INavigationProvider NavigationProvider { get; }
    protected object Category { get; }

    [ObservableProperty]
    private INavigatableModel? _currentModel;

    public Task NavigateToAsync<TModel>() where TModel : INavigatableModel
        => NavigateToAsync(typeof(TModel));

    public Task NavigateToAsync(Type type)
    {
        var model = NavigationProvider.GetNavigatableModel(type, Category);

        CurrentModel = model;

        return Task.CompletedTask;
    }

    public Task NavigateToAsync(INavigatableModel model)
    {
        CurrentModel = model;
        return Task.CompletedTask;
    }
}