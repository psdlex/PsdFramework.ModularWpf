using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Provider;

public sealed class NavigationProvider : INavigationProvider
{
    private readonly IServiceProvider _serviceProvider;

    public NavigationProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public INavigationModel GetNavigationModel(object category)
        => _serviceProvider.GetRequiredKeyedService<INavigationModel>(category);

    public INavigatableModel GetNavigatableModel<TModel>(object category) where TModel : INavigatableModel
        => _serviceProvider.GetRequiredKeyedService<TModel>(category);

    public INavigatableModel GetNavigatableModel(Type modelType, object category)
        => (INavigatableModel)_serviceProvider.GetRequiredKeyedService(modelType, category);

    public IEnumerable<INavigatableModel> GetNavigatableModels(object category)
        => _serviceProvider.GetKeyedServices<INavigatableModel>(category);
}