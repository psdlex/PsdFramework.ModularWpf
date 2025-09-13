using CommunityToolkit.Mvvm.ComponentModel;

using PsdFramework.ModularWpf.Navigations.Models;

namespace PsdFramework.ModularWpf.Navigations.Abstract;

public abstract partial class NavigationModelBase : ObservableObject, INavigationModel
{
    [ObservableProperty]
    private ObservableObject? _currentModel;

    protected HashSet<ObservableObject> CachedModels { get; set; } = [];

    public virtual Task NavigateToAsync<TModel>() where TModel : ObservableObject
        => NavigateToAsync(typeof(TModel));

    public virtual Task NavigateToAsync(Type type)
    {
        var model = CachedModels.FirstOrDefault(m => m.GetType() == type);

        if (model is null)
            return Task.CompletedTask;

        CurrentModel = model;
        return Task.CompletedTask;
    }

    public virtual Task NavigateToAsync(ObservableObject model)
    {
        if (CachedModels.Contains(model) == false)
            CachedModels.Add(model);

        CurrentModel = model;
        return Task.CompletedTask;
    }
}