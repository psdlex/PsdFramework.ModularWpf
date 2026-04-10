using PsdFramework.ModularWpf.Internal;
using System.Reflection;

namespace PsdFramework.ModularWpf.General;

public sealed class ComponentOptions
{
    private readonly OptionsBuildingManager _optionsBuildingManager = new();

    internal ComponentOptions()
    {
    }

    internal bool CacheByDefault => _optionsBuildingManager.GetValue<bool>(nameof(CacheByDefault));
    internal Func<Assembly, bool>? AssemblyFilter => _optionsBuildingManager.GetValue<Func<Assembly, bool>>(nameof(AssemblyFilter));
    internal IReadOnlyCollection<Type>? ConcreteTypes => _optionsBuildingManager.GetValue<IReadOnlyCollection<Type>>(nameof(ConcreteTypes));

    public static ComponentOptions Empty() => new();

    public ComponentOptions WithDefaultComponentCaching()
    {
        _optionsBuildingManager.ThrowOrContinueWithPropertyValue(nameof(CacheByDefault), true);
        return this;
    }

    public ComponentOptions WithAssemblyFilter(Func<Assembly, bool> assemblyFilter)
    {
        _optionsBuildingManager.ThrowOrContinueWithPropertyValue(nameof(AssemblyFilter), assemblyFilter);
        return this;
    }

    public ComponentOptions WithConcreteTypes(IReadOnlyCollection<Type> concreteTypes)
    {
        _optionsBuildingManager.ThrowOrContinueWithPropertyValue(nameof(ConcreteTypes), concreteTypes);
        return this;
    }
}
