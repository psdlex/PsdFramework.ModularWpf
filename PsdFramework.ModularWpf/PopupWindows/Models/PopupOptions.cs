using System.Windows;
using PsdFramework.ModularWpf.Models;

namespace PsdFramework.ModularWpf.PopupWindows.Models;

public sealed class PopupOptions
{
    public Window? Owner { get; init; }
    public bool CloseOnDeactivation { get; init; }
}