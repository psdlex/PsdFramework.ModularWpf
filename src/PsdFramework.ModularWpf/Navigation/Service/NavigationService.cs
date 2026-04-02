using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;
using System.Reflection;

namespace PsdFramework.ModularWpf.Navigation.Service;

internal sealed class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task NavigateAsync(NavigationOptions options)
    {
        if (options is null)
            throw new NullReferenceException("Options are null.");

        if (options.IsNavigatableSet == false)
            throw new InvalidOperationException($"Navigatable has not been set in the options. Please invoke '{nameof(NavigationOptions.ToNavigatable)}' method when providing options.");

        var navigatable = GetNavigatable(options);
        var navigationHost = GetNavigationHost(options);

        var context = new NavigationContext()
        {
            Navigatable = navigatable,
            NavigationHost = navigationHost,
            NavigatableDisplayName = GetNavigatableDisplayName(navigatable),
            Parameters = GetParameters(options)
        };

        // pipeline
        var previousNavigatable = navigationHost.CurrentModel;

        await navigationHost.OnNavigatingAsync(context);
        if (context.IsCancellationRequested)
            return;

        if (previousNavigatable is not null)
        {
            await previousNavigatable.OnNavigatingFromAsync(context);
            if (context.IsCancellationRequested)
                return;
        }

        await navigatable.OnNavigatingToAsync(context);
        if (context.IsCancellationRequested)
            return;

        // point of no return
        context.IsCancellationPossible = false;

        await navigatable.OnNavigatedToAsync(context);
        
        if (previousNavigatable is not null)
            await previousNavigatable.OnNavigatedFromAsync(context);
    
        await navigationHost.OnNavigatedAsync(context);
    }

    private INavigatable GetNavigatable(NavigationOptions options)
    {
        if (options.Navigatable is not null)
            return options.Navigatable;

        return _serviceProvider.GetRequiredKeyedService<INavigatable>(options.NavigatableType);
    }

    private INavigationHost GetNavigationHost(NavigationOptions options)
    {
        if (options.NavigationHost is not null)
            return options.NavigationHost;

        if (options.NavigationHostType is not null)
            return _serviceProvider.GetRequiredKeyedService<INavigationHost>(options.NavigationHostType);

        return _serviceProvider.GetRequiredKeyedService<INavigationHost>(options.Category);
    }

    private string? GetNavigatableDisplayName(INavigatable navigatable)
    {
        return navigatable
            .GetType()
            .GetCustomAttribute<NavigatableAttribute>()!
            .DisplayName;
    }

    private ContextualParameters GetParameters(NavigationOptions options)
    {
        if (options.ParameterConfiguration is null)
            return ContextualParameters.Empty;

        var parameters = new ContextualParameters();
        options.ParameterConfiguration.Invoke(parameters);

        return parameters;
    }
}
