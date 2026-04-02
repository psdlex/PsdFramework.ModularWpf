using System.Windows;

namespace PsdFramework.ModularWpf.ModuarApplication;

public abstract class ModularApplicationBase : Application, IModularApplication
{
    public IServiceProvider? ServiceProvider { get; protected set; }
}
