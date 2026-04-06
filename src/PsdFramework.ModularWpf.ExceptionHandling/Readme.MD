# PsdFramework.ModularWpf.ExceptionHandling
#### Exception handling extensions for the `PsdFramework.ModularWpf`

[![NuGet Version](https://img.shields.io/nuget/dt/PsdFramework.ModularWpf.Logging?style=flat&logo=nuget&label=NuGet)](https://www.nuget.org/packages/PsdFramework.ModularWpf.Logging)

## How to configure?
#### Your `App.xaml.cs` must implement `IApplicationModule` in order to use extensions methods. Or use the base class ModularApplication.
```csharp
public sealed partial class App : ModularApplication
{
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

#### Handling pipeline: `Exception` -> `DispatcherUnhandledException` -> `ExceptionHandlersController` -> `Specific Exception Handler` or `Global Handler`
#### Unhandled -> rethrow exception without handling it.