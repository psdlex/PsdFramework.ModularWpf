using System.Reflection;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Navigations.Models;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Service;

public sealed partial class NavigatorService : INavigatorService
{
    private readonly IServiceProvider _serviceProvider;

    public NavigatorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private async Task NavigateWithParameters(INavigationComponentModel navigation, INavigatableComponentModel navigatable, Action<ContextualParameters>? configureParameters)
    {
        var parameters = ConfigureParameters(configureParameters);
        var context = new NavigationContext()
        {
            NavigationComponentModel = navigation,
            NavigationType = navigation.GetType(),

            NavigatableComponentModel = navigatable,
            NavigatableType = navigatable.GetType(),
            NavigatableDisplayName = GetNavigatableDisplayName(navigatable),

            Parameters = parameters
        };

        // notifying the navigation about the beginning of a navigation
        await navigation.OnNavigating(context);
        if (context.IsCancellationRequested)
            return;

        // notifying the previous navigatable about the leaving
        if (navigation.CurrentModel is { } prev)
            await prev.OnNavigatingFrom(context);

        if (context.IsCancellationRequested)
            return;

        // cancellation is no longer possible
        context.IsCancellationPossible = false;

        await navigation.OnNavigated(context);
        await navigatable.OnNavigatedTo(context);
    }

    private string? GetNavigatableDisplayName(INavigatableComponentModel navigatable)
    {
        return navigatable
            .GetType()
            .GetCustomAttribute<NavigatableComponentModelAttribute>()!
            .DisplayName;
    }

    private ContextualParameters? ConfigureParameters(Action<ContextualParameters>? configureParameters)
    {
        if (configureParameters is null)
            return null;

        var parameters = new ContextualParameters();
        configureParameters(parameters);

        return parameters;
    }
}