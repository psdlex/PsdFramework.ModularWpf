using PsdFramework.ModularWpf.Popup.Models;
using System.Windows;

namespace PsdFramework.ModularWpf.Tests.Unit;

public class FakePopupWindow : FrameworkElement, IPopupWindow
{
    public Window Owner { get; set; } = null!;

    public event EventHandler Closed = null!;
    public event EventHandler Deactivated = null!;

    public virtual void Close()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }

    public virtual void Show()
    {
    }
}
