using System.Reflection;

namespace PsdFramework.ModularWpf.Internal;

internal static class ComponentUtilisersLocator
{
    private static IReadOnlyCollection<ComponentUtiliser>? _cachedUtilisers;

    public static IReadOnlyCollection<ComponentUtiliser> LocateAll()
    {
        return _cachedUtilisers ??= Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && t.IsAbstract == false && t.IsAssignableTo(typeof(ComponentUtiliser)))
            .Select(t => (ComponentUtiliser)Activator.CreateInstance(t)!)
            .ToList();
    }
}