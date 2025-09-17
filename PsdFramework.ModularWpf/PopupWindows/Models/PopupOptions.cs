using System.Windows;

namespace PsdFramework.ModularWpf.PopupWindows.Models;

public sealed class PopupOptions
{
    public Window? Owner { get; init; }
    public bool BlockOtherWindows { get; init; }
    public bool CloseOnDeactivation { get; init; }
}