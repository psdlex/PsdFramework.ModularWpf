using System.Windows.Controls;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

namespace PsdFramework.ModularWpf.Vvm;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services)
        where TView : Control
        where TViewModel : ObservableObject
    {
        services.AddTransient<TView>();
        services.AddViewModel<TViewModel>();

        services.AddTransient<IView<TView, TViewModel>>(provider =>
        {
            var viewModel = provider.GetRequiredService<TViewModel>();
            var view = provider.GetRequiredService<TView>();
            view.DataContext = viewModel;

            return new ViewRepresentation<TView, TViewModel>(view, viewModel);
        });

        services.AddTransient<IView<TView>>(provider => provider.GetRequiredService<IView<TView, TViewModel>>());

        return services;
    }

    public static IServiceCollection AddCachedView<TView, TViewModel>(this IServiceCollection services)
        where TView : Control, new()
        where TViewModel : ObservableObject
    {
        services.AddScoped<TView>();
        services.AddCachedViewModel<TViewModel>();

        services.AddScoped<IView<TView, TViewModel>>(provider =>
        {
            var viewModel = provider.GetRequiredService<TViewModel>();
            var view = provider.GetRequiredService<TView>();
            view.DataContext = viewModel;

            return new ViewRepresentation<TView, TViewModel>(view, viewModel);
        });

        services.AddScoped<IView<TView>>(provider => provider.GetRequiredService<IView<TView, TViewModel>>());

        return services;
    }

    public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services)
        where TViewModel : ObservableObject
    {
        services.AddTransient<TViewModel>();
        return services;
    }

    public static IServiceCollection AddCachedViewModel<TViewModel>(this IServiceCollection services)
        where TViewModel : ObservableObject
    {
        services.AddScoped<TViewModel>();
        return services;
    }
}