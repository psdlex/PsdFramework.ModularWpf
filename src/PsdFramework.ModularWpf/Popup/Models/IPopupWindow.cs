using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace PsdFramework.ModularWpf.Popup.Models;

public interface IPopupWindow
{
    event EventHandler Closed;
    event EventHandler Deactivated;

    object DataContext { get; set; }

    Dispatcher Dispatcher { get; }
    Window Owner { get; set; }

    void Show();
    void Close();
}
