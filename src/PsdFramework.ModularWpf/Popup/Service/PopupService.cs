using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Popup.Models;
using PsdFramework.ModularWpf.Popup.Models.Result;

namespace PsdFramework.ModularWpf.Popup.Service;

internal sealed class PopupService : IPopupService
{
    private readonly IServiceProvider _serviceProvider;

    public PopupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<PopupResult<TResult>> ShowPopupAsync<TPopup, TWindow, TResult>()
        where TPopup : class, IPopup<TWindow, TResult>
        where TWindow : class, IPopupWindow, new()
    {
        return ShowPopupAsync<TPopup, TWindow, TResult>(PopupOptions.Empty());
    }

    public async Task<PopupResult<TResult>> ShowPopupAsync<TPopup, TWindow, TResult>(PopupOptions options)
        where TPopup : class, IPopup<TWindow, TResult>
        where TWindow : class, IPopupWindow, new()
    {
        var popup = _serviceProvider.GetRequiredKeyedService<IPopup<TWindow, TResult>>(typeof(TPopup));
        var window = new TWindow();
        var session = new PopupSession<IPopup<TWindow, TResult>, TWindow, TResult>(popup, window, options);

        window.DataContext = popup;

        return await session.ShowAsync(GetParameters(options));
    }

    private ContextualParameters GetParameters(PopupOptions options)
    {
        if (options.ParameterConfiguration is null)
            return ContextualParameters.Empty;

        var parameters = new ContextualParameters();
        options.ParameterConfiguration.Invoke(parameters);

        return parameters;
    }
}