using PsdFramework.ModularWpf.Popup.Models;
using System.Windows;

namespace SampleProject.Popup;

// You dont have to implement anything. IPopupWindow bases off the properties and events
// that Window already contains. But the purpose of it is to provide testability.
public partial class MyPopupWindow : Window, IPopupWindow
{
    public MyPopupWindow()
    {
        InitializeComponent();
    }
}
