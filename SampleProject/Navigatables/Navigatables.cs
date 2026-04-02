using PsdFramework.ModularWpf.Navigation.Navigatable;

namespace SampleProject.Navigatables;

[Navigatable(IsCached = true)]
public sealed class SingletonNavigatable : NavigatableBase;

[Navigatable(IsCached = false)]
public sealed class TransientNavigatable : NavigatableBase;