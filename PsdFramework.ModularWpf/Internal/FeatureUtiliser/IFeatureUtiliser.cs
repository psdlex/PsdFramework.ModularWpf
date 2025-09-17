using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.General.Registration;

namespace PsdFramework.ModularWpf.Internal.FeatureUtiliser;

internal interface IFeatureUtiliser
{
    bool TryUtilise(IServiceCollection services, ComponentModelType type);
}