using System.Reflection;

namespace PsdFramework.ModularWpf.Internal.FeatureUtiliser;

internal static class FeatureUtilisersLocator
{
    public static IReadOnlyList<IFeatureUtiliser> FindAll()
    {
        return Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i == typeof(IFeatureUtiliser)))
            .Select(t => (IFeatureUtiliser)Activator.CreateInstance(t)!)
            .ToList();
    }
}