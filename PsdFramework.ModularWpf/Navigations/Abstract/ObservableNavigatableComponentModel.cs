using CommunityToolkit.Mvvm.ComponentModel;
using PsdFramework.ModularWpf.Navigations.Models;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;

namespace PsdFramework.ModularWpf.Navigations.Abstract;

public abstract partial class ObservableNavigatableComponentModel : ObservableObject, INavigatableComponentModel
{
    public virtual Task OnNavigatedTo(NavigationContext context) => Task.CompletedTask;
    public virtual Task OnNavigatingFrom(NavigationContext context) => Task.CompletedTask;
}