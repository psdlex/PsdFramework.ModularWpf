namespace PsdFramework.ModularWpf.General.Registration;

internal sealed record ComponentModelType(
    Type ModelType,
    bool IsCached
);