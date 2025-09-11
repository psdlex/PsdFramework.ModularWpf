# PsdFramework.ModularWpf.ExceptionHandling

Exception handling extensions for the `PsdFramework.ModularWpf`

## How to configure?
#### First, your `App.xaml.cs` must implement `IApplicationModule` in order to use extensions methods.
```csharp
public sealed partial class App : System.Windows.Application, IModularApplication
{
    public IServiceProvider? ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        this.AddGlobalExceptionHandler(); // Adding handler of dispatcher unhanhdled exceptions

        var services = new ServiceProvider();

        services.AddExceptionHandlersController(); // controller of exception handlers
        services.AddExceptionHandler<GlobalExceptionHandler>(); // adding global exception handler
        // add more handlers if needed
    }
}
```

Handling pipeline: `Exception` -> `DispatcherUnhandledException` -> `ExceptionHandlersController` -> `Specific Exception Handler` or `Global Handler`
Unhandled -> rethrow exception without handling it.