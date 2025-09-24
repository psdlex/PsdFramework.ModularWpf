using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;
using PsdFramework.ModularWpf.Parameters;

namespace PsdFramework.ModularWpf.Navigations.Service;

public sealed partial class NavigatorService : INavigatorService
{
    private readonly IServiceProvider _serviceProvider;

    public NavigatorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private Task NavigateWithParameters<TNavigatable>(INavigationComponentModel navigation, INavigatableComponentModel navigatable, Action<ParameterBuilder<TNavigatable>>? configureBuilder)
        where TNavigatable : INavigatableComponentModel
    {
        if (configureBuilder is not null)
        {
            var builder = new ParameterBuilder<TNavigatable>();
            configureBuilder(builder);
            builder.ApplyPropertyValues((TNavigatable)navigatable);
        }

        return Navigate(navigation, navigatable);
    }

    private Task NavigateWithParameters(INavigationComponentModel navigation, INavigatableComponentModel navigatable, Action<ParameterBuilder>? configureBuilder)
    {
        if (configureBuilder is not null)
        {
            var builder = new ParameterBuilder(navigatable.GetType());
            configureBuilder(builder);
            builder.ApplyPropertyValues(navigatable);
        }

        return Navigate(navigation, navigatable);
    }

    private async Task Navigate(INavigationComponentModel navigation, INavigatableComponentModel navigatable)
    {
        await navigation.OnPreNavigated(navigatable);

        navigation.CurrentModel?.OnNavigatedFrom(navigation);
        await navigation.OnNavigated(navigatable);
        await navigatable.OnNavigatedTo(navigation);
    }
}