using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsdFramework.ModularWpf.General;
using PsdFramework.ModularWpf.Navigation.Abstract;
using PsdFramework.ModularWpf.Navigation.NavigationHost;
using PsdFramework.ModularWpf.Navigation.Service;
using PsdFramework.ModularWpf.Popup.Models.Result;
using PsdFramework.ModularWpf.Popup.Service;
using PsdFramework.ModularWpf.View.Models;
using SampleProject.Navigatables;
using SampleProject.Popup;
using System.Windows;

namespace SampleProject;

[SharedComponentModel]
[NavigationHost(IsCached = true)]
[View<MainWindow>(IsCached = true)]
public sealed partial class MainWindowViewModel : ObservableNavigationHostBase
{
    private readonly INavigationService _navigationService;
    private readonly IPopupService _popupService;

    public MainWindowViewModel(INavigationService navigationService, IPopupService popupService)
    {
        _navigationService = navigationService;
        _popupService = popupService;
    }

    [RelayCommand]
    private Task OnNavigateToSingletonNavigatableAsync()
        => _navigationService.NavigateAsync(NavigationOptions.FromNavigationHost(this).ToNavigatable<SingletonNavigatable>());


    [RelayCommand]
    private Task OnNavigateToTransientNavigatableAsync()
        => _navigationService.NavigateAsync(NavigationOptions.FromNavigationHost(this).ToNavigatable<TransientNavigatable>());

    [RelayCommand]
    private async Task OnShowPopupAsync()
    {
        var result = await _popupService.ShowPopupAsync<MyPopup, MyPopupWindow, MyPopupResult>(PopupOptions.Empty().WithOwner(App.Current.MainWindow));
    
        if (result.ExitBasis == PopupExitBasis.ExternalTermination)
        {
            // Dont use MessageBox directly in a real project. Abstract it away using a DialogService to provide the testability.
            MessageBox.Show("Popup was closed externally. No result returned.");
            return;
        }

        MessageBox.Show($"Popup was closed intentionally. It was active for {(int)result.Value.ActiveDuration.TotalSeconds} seconds!");
    }
}
