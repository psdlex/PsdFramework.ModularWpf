using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.General;

namespace PsdFramework.ModularWpf.Internal;

internal abstract class ComponentUtiliser
{
    protected ComponentUtiliser(Type attributeType)
    {
        if (attributeType.IsAssignableTo(typeof(ComponentAttribute)) == false)
            ExceptionHelper.ThrowAttributeIsNotComponent(attributeType);

        AttributeType = attributeType;
    }

    public Type AttributeType { get; }
    public abstract void Utilise(IServiceCollection services, ComponentDescription description);
}

internal abstract class ComponentUtiliser<TAttribute> : ComponentUtiliser
    where TAttribute : ComponentAttribute
{
    protected ComponentUtiliser() : base(typeof(TAttribute))
    { }
}