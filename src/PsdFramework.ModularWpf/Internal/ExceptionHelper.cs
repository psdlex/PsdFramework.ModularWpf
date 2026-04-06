using PsdFramework.ModularWpf.General;
using System.Diagnostics.CodeAnalysis;

namespace PsdFramework.ModularWpf.Internal;

internal static class ExceptionHelper
{
    [DoesNotReturn]
    public static void ThrowAttributeNotComponent(Type attributeType)
        => throw new InvalidOperationException($"Attribute '{attributeType}' is not derived from '{typeof(ComponentAttribute)}'.");

    [DoesNotReturn]
    public static void ThrowComponentModelInterfaceImplementationRequired(Type attributeType, Type interfaceType)
        => throw new NotImplementedException($"Attribute '{attributeType}' requires a model to implement '{interfaceType}' interface.");

}
