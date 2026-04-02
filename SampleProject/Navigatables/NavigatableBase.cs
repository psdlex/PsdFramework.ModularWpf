using PsdFramework.ModularWpf.Navigation.Abstract;

namespace SampleProject.Navigatables;

public abstract class NavigatableBase : ObservableNavigatableBase
{
    public Guid Id { get; } = Guid.NewGuid();
}
