using PsdFramework.ModularWpf.Internal;
using PsdFramework.ModularWpf.Models;
using System.Windows;

namespace PsdFramework.ModularWpf.Popup;

public sealed class PopupOptions
{
    private readonly OptionsBuildingManager _optionsBuildingManager = new();

    internal PopupOptions()
    {
    }

    internal Window? Owner => _optionsBuildingManager.GetValue<Window>(nameof(Owner));
    internal bool CloseOnDeactivation => _optionsBuildingManager.GetValue<bool>(nameof(CloseOnDeactivation));
    internal Action<ContextualParametersBuilder>? ParametersBuilderConfiguration
        => _optionsBuildingManager.GetValue<Action<ContextualParametersBuilder>>(nameof(ParametersBuilderConfiguration));

    public static PopupOptions Empty() => new();

    public PopupOptions WithOwner(Window owner)
    {
        _optionsBuildingManager.ThrowOrContinueWithPropertyValue(nameof(Owner), owner);
        return this;
    }

    public PopupOptions WithClosingOnDeactivation()
    {
        _optionsBuildingManager.ThrowOrContinueWithPropertyValue(nameof(CloseOnDeactivation), true);
        return this;
    }

    public PopupOptions WithParameters(Action<ContextualParametersBuilder> parametersConfiguration)
    {
        _optionsBuildingManager.ThrowOrContinueWithPropertyValue(nameof(ParametersBuilderConfiguration), parametersConfiguration);
        return this;
    }
}