using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.PopupWindows.Service;

namespace PsdFramework.ModularWpf.PopupWindows;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPopupWindowService(this IServiceCollection services)
        => services.AddScoped<IPopupWindowService, PopupWindowService>();
}