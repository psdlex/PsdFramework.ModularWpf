using PsdFramework.ModularWpf.Models;
using System.Windows;

namespace PsdFramework.ModularWpf.Popup.Service;

public sealed class PopupOptions
{
    private bool _isOwnerSet, _isDeactivationBehaviorSet, _areParametersConfigured;

    internal PopupOptions()
    {
    }

    internal Window? Owner { get; private set; }
    internal bool CloseOnDeactivation { get; private set; }
    internal Action<ContextualParametersBuilder>? ParametersBuilderConfiguration { get; private set; }

    public static PopupOptions Empty() => new();

    public PopupOptions WithOwner(Window owner)
    {
        if (_isOwnerSet)
            throw new InvalidOperationException("Owner is already set.");

        _isOwnerSet = true;
        Owner = owner;
        return this;
    }

    public PopupOptions WithClosingOnDeactivation()
    {
        if (_isDeactivationBehaviorSet)
            throw new InvalidOperationException("Closing on deactivation is already set.");

        _isDeactivationBehaviorSet = true;
        CloseOnDeactivation = true;
        return this;
    }

    public PopupOptions WithParameters(Action<ContextualParametersBuilder> parametersConfiguration)
    {
        if (_areParametersConfigured)
            throw new InvalidOperationException("Parameters are already set.");

        _areParametersConfigured = true;
        ParametersBuilderConfiguration = parametersConfiguration;
        return this;
    }
}