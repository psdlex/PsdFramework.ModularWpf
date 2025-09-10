using System.Windows.Controls;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

namespace PsdFramework.ModularWpf.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services)
        where TView : Control, new()
        where TViewModel : ObservableObject
    {
        services.AddTransient<TView>(provider => new() { DataContext = provider.GetRequiredService<TViewModel>() });
        services.AddViewModel<TViewModel>();

        return services;
    }

    public static IServiceCollection AddCachedView<TView, TViewModel>(this IServiceCollection services)
        where TView : Control, new()
        where TViewModel : ObservableObject
    {
        services.AddScoped<TView>(provider => new() { DataContext = provider.GetRequiredService<TViewModel>() });
        services.AddViewModel<TViewModel>();

        return services;
    }

    public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services)
        where TViewModel : ObservableObject
    {
        services.AddTransient<TViewModel>();
        return services;
    }
}