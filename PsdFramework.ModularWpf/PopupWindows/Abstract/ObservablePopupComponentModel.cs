using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.PopupWindows.Models;
using PsdFramework.ModularWpf.PopupWindows.Models.Result;

namespace PsdFramework.ModularWpf.PopupWindows.Abstract;

public abstract partial class ObservablePopupComponentModel<TPopup, TResult> : ObservableObject, IPopupComponentModel<TPopup, TResult>, IValidatableDataContext
    where TPopup : Window, new()
{
    private readonly TaskCompletionSource<PopupResult<TResult>> _completionSource;
    private DependencyObject? _container;

    protected ObservablePopupComponentModel()
    {
        _completionSource = new();
    }

    public virtual Task OnPopupOpened(ContextualParameters? parameters)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnPopupClosed()
    {
        _completionSource.TrySetResult(new PopupResult<TResult>(PopupExitBasis.Unexpected, default));
        return Task.CompletedTask;
    }

    public Task<PopupResult<TResult>> GetResult() => _completionSource.Task;

    protected virtual void SetResult(TResult result)
    {
        _completionSource.TrySetResult(new PopupResult<TResult>(PopupExitBasis.Intentional, result));
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

    private IReadOnlyList<ValidationError> GetValidationErrors(DependencyObject d)
    {
        List<ValidationError> errors = [];
        errors.AddRange(Validation.GetErrors(d));

        // Recurse through children
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
        {
            var child = VisualTreeHelper.GetChild(d, i);
            errors.AddRange(GetValidationErrors(child));
        }

        return errors;
    }

    [RelayCommand]
    private void OnRegisterValidationContainer(DependencyObject container) => _container = container;
}