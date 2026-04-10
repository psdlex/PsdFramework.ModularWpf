using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Interceptors;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Popup.Models;
using PsdFramework.ModularWpf.Popup.Models.Result;

namespace PsdFramework.ModularWpf.Popup.Service;

internal sealed class PopupService : IPopupService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEnumerable<IInterceptor<IPopupService>> _interceptors;

    public PopupService(IServiceProvider serviceProvider, IEnumerable<IInterceptor<IPopupService>> interceptors)
    {
        _serviceProvider = serviceProvider;
        _interceptors = interceptors;
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
        await InterceptorHelper.InterceptAllAsync(_interceptors, InterceptorHelper.InterceptionPhase.PreExecution);

        var popup = _serviceProvider.GetRequiredKeyedService<IPopup<TWindow, TResult>>(typeof(TPopup));
        var window = new TWindow();
        var session = new PopupSession<IPopup<TWindow, TResult>, TWindow, TResult>(popup, window, options);

        window.DataContext = popup;

        var result = await session.ShowAsync(GetParameters(options));
    
        await InterceptorHelper.InterceptAllAsync(_interceptors, InterceptorHelper.InterceptionPhase.PostExecution);

        return result;
    }

    private ContextualParameters GetParameters(PopupOptions options)
    {
        if (options.ParametersBuilderConfiguration is null)
            return ContextualParameters.Empty();

        var builder = new ContextualParametersBuilder();
        options.ParametersBuilderConfiguration.Invoke(builder);

        return builder.Build();
    }
}