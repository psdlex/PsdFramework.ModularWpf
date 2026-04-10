using PsdFramework.ModularWpf.General;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PsdFramework.ModularWpf.Internal;

internal static class ComponentUtility
{
    public static bool TryCreateDescription(Type modelType, Type attributeType, bool isShared, bool isCachedByDefault, [NotNullWhen(true)] out ComponentDescription? description)
    {
        description = null;

        if (attributeType.IsAssignableTo(typeof(ComponentAttribute)) == false)
            ExceptionHelper.ThrowAttributeIsNotComponent(attributeType);

        if (modelType.GetCustomAttribute(attributeType) is not ComponentAttribute attribute)
            return false;

        description = new ComponentDescription(
            ModelType: modelType,
            Attribute: attribute,
            IsSharedModel: isShared,
            IsCached: IsCached(attribute, isCachedByDefault)
        );

        if (isShared && description.IsCached == false)
            throw new InvalidOperationException($"A component of model '{modelType}' cannot be transient (not cached), as the model is marked as shared.");

        return true;
    }

    public static bool IsSharedComponentModel(Type modelType) => modelType.IsDefined(typeof(SharedComponentModelAttribute), inherit: false);

    private static bool IsCached(ComponentAttribute attribute, bool isCachedByDefault)
    {
        if (attribute.IsCachingExplicitelyDefined)
            return attribute.IsCached;

        return isCachedByDefault;
    }
}
