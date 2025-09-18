using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.PopupWindows.Models;
using PsdFramework.ModularWpf.PopupWindows.Models.Result;
using PsdFramework.ModularWpf.PopupWindows.Service.Managers;

namespace PsdFramework.ModularWpf.PopupWindows.Service;

public sealed class PopupWindowService : IPopupWindowService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PopupWindowService> _logger;
    private readonly WindowStateManager _windowStateManager;

    public PopupWindowService(IServiceProvider serviceProvider, ILogger<PopupWindowService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        _windowStateManager = new();
    }

    public Task<PopupResult<TResult>> ShowPopup<TComponentModel, TPopup, TResult>()
        where TComponentModel : class, IPopupComponentModel<TPopup, TResult>
        where TPopup : Window, new()
    {
        return ShowPopup<TComponentModel, TPopup, TResult>(new PopupOptions());
    }

    public async Task<PopupResult<TResult>> ShowPopup<TComponentModel, TPopup, TResult>(PopupOptions options)
        where TComponentModel : class, IPopupComponentModel<TPopup, TResult>
        where TPopup : Window, new()
    {
        _logger.LogDebug("Showing new popup: " + typeof(TPopup).Name);

        var componentModel = (TComponentModel)_serviceProvider.GetRequiredKeyedService<IComponentModel>(typeof(TComponentModel));
        var popup = CreatePopup<TPopup>(componentModel);

        TrySetOwner(options, popup);
        ConfigureEvents<TComponentModel, TPopup, TResult>(componentModel, popup, options);

        var resultTask = componentModel.GetResult();
        _ = resultTask.ContinueWith(_ => 
            popup.Dispatcher.Invoke(() => SafeClose(popup)), 
            TaskScheduler.Default
        );

        popup.Show();

        var result = await resultTask;

        _windowStateManager.DisposeState(popup);
        return result;
    }

    private TPopup CreatePopup<TPopup>(object dataContext) 
        where TPopup : Window, new()
    {
        var popup = new TPopup();
        popup.DataContext = dataContext;
        return popup;
    }

    private void ConfigureEvents<TComponentModel, TPopup, TResult>(TComponentModel componentModel, Window popup, PopupOptions options)
        where TComponentModel : class, IPopupComponentModel<TPopup, TResult>
        where TPopup : Window, new()
    {
        popup.Closing += (_, _) =>
        {
            _windowStateManager.SetClosingState(popup);
            componentModel.OnPopupExit();
        };

        if (options.CloseOnDeactivation)
            popup.Deactivated += (s, e) => SafeClose(popup);
    }

    private void TrySetOwner(PopupOptions options, Window window)
    {
        if (options.Owner is null)
            return;

        window.Owner = options.Owner;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    }

    private void SafeClose(Window window)
    {
        if (_windowStateManager.IsClosing(window))
            return;

        _windowStateManager.SetClosingState(window);
        window.Close();
    }
}