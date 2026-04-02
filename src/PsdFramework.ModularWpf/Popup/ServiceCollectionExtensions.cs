using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Popup.Service;

namespace PsdFramework.ModularWpf.Popup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPopupService(this IServiceCollection services)
        => services.AddScoped<IPopupService, PopupService>();
}