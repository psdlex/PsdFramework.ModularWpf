using PsdFramework.ModularWpf.General;

namespace PsdFramework.ModularWpf.Internal;

internal sealed record ComponentDescription(Type ModelType, ComponentAttribute Attribute, bool IsSharedModel, bool IsCached);