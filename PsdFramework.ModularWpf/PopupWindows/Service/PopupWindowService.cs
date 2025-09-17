using System.Text.Json.Serialization.Metadata;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.PopupWindows.Models;
using PsdFramework.ModularWpf.PopupWindows.Models.Result;

namespace PsdFramework.ModularWpf.PopupWindows.Service;

public sealed class PopupWindowService : IPopupWindowService
{
    private readonly IServiceProvider _serviceProvider;

    public PopupWindowService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
        var componentModel = (TComponentModel)_serviceProvider.GetRequiredKeyedService<IComponentModel>(typeof(TComponentModel));
        var popup = new TPopup();
        popup.DataContext = componentModel;

        TrySetOwner(options, popup);
        ConfigureEvents<TComponentModel, TPopup, TResult>(componentModel, popup, options);

        var isClosed = false;
        popup.Closed += (_, _) =>
        {
            isClosed = true;
            componentModel.OnPopupExit();
        };

        var display = GetDisplayFunction(popup, options);
        display();

        var result = await componentModel.GetResult();

        if (isClosed == false)
            popup.Close();

        return result;
    }

    private void TrySetOwner(PopupOptions options, Window window)
    {
        if (options.Owner is null)
            return;

        window.Owner = options.Owner;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    }

    private void ConfigureEvents<TComponentModel, TPopup, TResult>(TComponentModel componentModel, Window window, PopupOptions options)
        where TComponentModel : class, IPopupComponentModel<TPopup, TResult>
        where TPopup : Window, new()
    {
        if (options.CloseOnDeactivation)
            window.Deactivated += (s, e) => window.Close();
    }

    private Action GetDisplayFunction(Window window, PopupOptions options)
    {
        return options.BlockOtherWindows
            ? () => window.ShowDialog()
            : window.Show;
    }
}