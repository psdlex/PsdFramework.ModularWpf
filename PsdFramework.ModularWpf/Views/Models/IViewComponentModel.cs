using System.Windows;

using PsdFramework.ModularWpf.General.Models.Components;

namespace PsdFramework.ModularWpf.Views.Models;

public interface IViewComponentModel<TView> : IComponentModel
    where TView : FrameworkElement;