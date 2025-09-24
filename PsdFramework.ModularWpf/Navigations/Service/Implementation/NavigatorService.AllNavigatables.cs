using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Parameters;

namespace PsdFramework.ModularWpf.Navigations.Service;

partial class NavigatorService
{
    public IEnumerable<INavigatableComponentModel> GetAllNavigatables(object category)
    {
        return _serviceProvider.GetKeyedServices<INavigatableComponentModel>(category);
    }

    public IEnumerable<INavigatableComponentModel> GetAllNavigatables(object category, Action<ParameterBuilder> configureParameters)
    {
        var navigatables = GetAllNavigatables(category);

        foreach (var navigatable in navigatables)
        {
            var builder = new ParameterBuilder(navigatable.GetType());
            configureParameters(builder);
            builder.ApplyPropertyValues(navigatable);

            yield return navigatable;
        }
    }
}