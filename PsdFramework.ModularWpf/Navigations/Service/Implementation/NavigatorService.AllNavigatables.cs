using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Navigations.Models;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;

namespace PsdFramework.ModularWpf.Navigations.Service;

partial class NavigatorService
{
    public IEnumerable<INavigatableComponentModel> GetAllNavigatables(object category)
    {
        return _serviceProvider.GetKeyedServices<INavigatableComponentModel>(category);
    }
}