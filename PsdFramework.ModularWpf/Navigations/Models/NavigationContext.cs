using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Models;

public sealed class NavigationContext
{
    internal NavigationContext()
    {
    }

    internal bool IsCancellationPossible { get; set; } = true;
    internal bool IsCancellationRequested { get; private set; }

    public required Type NavigationType { get; init; }
    public required Type NavigatableType { get; init; }

    public required INavigationComponentModel NavigationComponentModel { get; init; }
    public required INavigatableComponentModel NavigatableComponentModel { get; init; }

    public string? NavigatableDisplayName { get; init; }
    public ContextualParameters? Parameters { get; init; }

    public void Cancel()
    {
        if (IsCancellationPossible == false)
            throw new InvalidOperationException("Cancellation is not possible at this stage.");

        IsCancellationRequested = true;
    }
}