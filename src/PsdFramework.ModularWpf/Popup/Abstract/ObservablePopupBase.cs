using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Popup.Models;
using PsdFramework.ModularWpf.Popup.Models.Result;

namespace PsdFramework.ModularWpf.Popup.Abstract;

public abstract class ObservablePopupBase<TWindow, TResult> : ObservableObject, IPopup<TWindow, TResult>, IValidatableDataContext
    where TWindow : IPopupWindow, new()
{
    private readonly TaskCompletionSource<PopupResult<TResult>> _resultCompletionSource = new();
    private DependencyObject? _container;

    protected ObservablePopupBase()
    {
        RegisterValidatableContainerCommand = new RelayCommand<DependencyObject>(OnRegisterValidatableContainer);
    }

    public IRelayCommand<DependencyObject> RegisterValidatableContainerCommand { get; }

    public virtual Task OnOpenedAsync(ContextualParameters parameters) => Task.CompletedTask;
    public virtual Task OnClosedAsync()
    {
        _resultCompletionSource.TrySetResult(new PopupResult<TResult>(PopupExitBasis.ExternalTermination, default));
        return Task.CompletedTask;
    }

    public Task<PopupResult<TResult>> GetResultAsync() => _resultCompletionSource.Task;

    protected virtual void SetResult(TResult result)
    {
        _resultCompletionSource.TrySetResult(new PopupResult<TResult>(PopupExitBasis.Intentional, result));
    }

    /// <summary>
    /// Must be used alongside the ValidationContainerBehavior
    /// </summary>
    protected IReadOnlyList<ValidationError> GetValidationErrors()
    {
        if (_container is null)
            return [];

        return GetValidationErrors(_container);
    }

    private void OnRegisterValidatableContainer(DependencyObject? container) => _container = container;

    private static IReadOnlyList<ValidationError> GetValidationErrors(DependencyObject container)
    {
        var errors = new List<ValidationError>();
        errors.AddRange(Validation.GetErrors(container));

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(container); i++)
        {
            var child = VisualTreeHelper.GetChild(container, i);
            errors.AddRange(GetValidationErrors(child));
        }

        return errors;
    }
}