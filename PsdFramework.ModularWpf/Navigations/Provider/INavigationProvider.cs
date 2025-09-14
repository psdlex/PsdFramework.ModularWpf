using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Provider;
public interface INavigationProvider
{
    INavigationModel GetNavigationModel(object category);
    INavigatableModel GetNavigatableModel<TModel>(object category) where TModel : INavigatableModel;
    INavigatableModel GetNavigatableModel(Type modelType, object category);
    IEnumerable<INavigatableModel> GetNavigatableModels(object category);
}