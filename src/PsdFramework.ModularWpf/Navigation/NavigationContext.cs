using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;

namespace PsdFramework.ModularWpf.Navigation;

public sealed class NavigationContext
{
    internal NavigationContext()
    {
    }

    internal bool IsCancellationPossible { get; set; } = true;
    internal bool IsCancellationRequested { get; private set; }

    public required INavigationHost NavigationHost { get; init; }
    public required INavigatable Navigatable { get; init; }

    public string? NavigatableDisplayName { get; init; }
    public ContextualParameters Parameters { get; init; } = ContextualParameters.Empty();

    public void CancelNavigation()
    {
        if (IsCancellationPossible == false)
            throw new InvalidOperationException("Cancellation is not possible at this phase.");

        IsCancellationRequested = true;
    }
}