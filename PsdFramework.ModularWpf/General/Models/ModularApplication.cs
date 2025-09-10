using System.Windows;

namespace PsdFramework.ModularWpf.General.Models;

public abstract class ModularApplication : Application
{
    public IServiceProvider? ServiceProvider { get; }
}