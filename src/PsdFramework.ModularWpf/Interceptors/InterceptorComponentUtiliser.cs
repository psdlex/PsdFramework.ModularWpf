using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PsdFramework.ModularWpf.Internal;

namespace PsdFramework.ModularWpf.Interceptors;

internal sealed class InterceptorComponentUtiliser : ComponentUtiliser<InterceptorAttribute>
{
    public override void Utilise(IServiceCollection services, ComponentDescription description)
    {
        if (description.ModelType
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInterceptor<>)) is not { } interceptorInterface)
        {
            ExceptionHelper.ThrowComponentModelInterfaceImplementationRequired(typeof(InterceptorAttribute), typeof(IInterceptor<>));
            return;
        }

        if (description.IsSharedModel)
            throw new InvalidOperationException("Interceptors should not be shared component models.");

        services.TryAddEnumerable(new ServiceDescriptor(
            serviceType: interceptorInterface,
            implementationType: description.ModelType,
            lifetime: description.IsCached ? ServiceLifetime.Singleton : ServiceLifetime.Transient
        ));
    }
}
