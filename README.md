# PsdFramework.ModularWpf

[![NuGet Version](https://img.shields.io/nuget/dt/PsdFramework.ModularWpf?style=flat&logo=nuget&label=NuGet)](https://www.nuget.org/packages/PsdFramework.ModularWpf)
[![Exception Handler](https://img.shields.io/badge/Github-Exception_Handling-green?style=flat&logo=github)](https://www.github.com/psdlex/PsdFramework.ModularWpf/tree/master/PsdFramework.ModularWpf.ExceptionHandling)
[![Logging](https://img.shields.io/badge/Github-Logging-green?style=flat&logo=github)](https://www.github.com/psdlex/PsdFramework.ModularWpf/tree/master/PsdFramework.ModularWpf.Logging)

#### `ModularWpf` is based on the `CommunityToolkit.Mvvm` package. A framework library that helps with modular design in a WPF application.

## Features
- Scalable and convenient navigation model for navigating between the view models.
- Abstract support for dialog windows with models.
- Automatic View-ViewModel binder.
- Service interceptors that allow you to extend the functionality of a given service.
- Validation container behavior for the popup windows.
- Everything is built on top of `Microsoft.Extensions.DependencyInjection` for view-models which makes it easy to integrate into an existing application using this DI framework.

#### Additional
- Easy-to-use app-level exception handler.
- Very simple logger for minimal tasks.

## How to setup your code-base
For the complete experience, inherit the `IModularApplication` interface in your `App.xaml.cs` file.
You can do it by using the `ModularApplicationBase` class, or by implementing the interface yourself.
This way you can access the extension methods provided by the framework.

### App.xaml.cs
```csharp
public sealed partial class App : ModularApplication
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Extension method for IModularApplication.
        // Requires the PsdFramework.ModularWpf.ExceptionHandling package.
        this.AddGlobalExceptionHandler();

        var services = new ServiceCollection();
        services.AddComponents(); // The step that includes the components

        // This property is contained within the ModularApplication base class.
        ServiceProvider = services.BuildServiceProvider();
    }
}
```

## How to setup the MainWindow as an MVVM-based model
We need to remove the `StartupUri` from the `App.xaml` file and register the `MainViewModel` as a component model marked using the `[View<TWindow>]` attribute.

#### App.xaml
```xml
<Application
    StartupUri="..." <!-- Remove this line -->
```

#### MainViewModel.cs
```csharp
[View<MainWindow>]
public sealed partial class MainViewModel : ObservableObject;
```

#### App.xaml.cs
```csharp
protected override void OnStartup()
{
    ...

    var mainView = ServiceProvider.GetRequiredService<IView<MainWindow>>();

    MainWindow = mainView.View;
    MainWindow.Show();
}
```

`IView<TView>` contains the `View` property where `TView` is a `FrameworkElement`.
If you want to access `IView`'s model as well, then use the `IView<TView, TModel>` interface. *(Ex: `IView<MainWindow, MainViewModel>().Model`)*

## Types of components
- `[NavigationHost]` - component that marks the model as a navigation host. A model that contains navigatable models and is considered as a "browser".
- `[Navigatable]` - component that marks the model as a navigatable. A model that can be navigated to/from.
- `[Popup<TWindow>]` - component that marks the model as a popup/dialog. A model that can be used as a context for a dialog window.
- `[View<TWindow>]` - component that marks the model as a context for a specific view.

## Types of services
- `INavigationService` - service that manages the navigations within a **navigation host** using a **navigatable model**.
- `IPopupService` - service that manages the dialog/popup windows using a **popup model** and a **popup window**.

## How to setup the navigation
First of all, we need to define a navigation host that will control the navigatables.
Then we need to mark the model using the `[NavigationHost]` attribute.
After that, you can implement the `INavigationHost` directly, but there is a lot of code to implement, so its preferable to use the built-in base class instead.
`ObservableNavigationHostBase` - abstract class that implements `INavigationHost` and extends the `ObservableObject` class to provide observable properties for bindings.

```csharp
[NavigationHost(Category = (object)"some category")] // Category is not mandatory
public sealed partial class MyNavigationHost : ObservableNavigationHostBase
{
    public override Task OnNavigatingAsync(NavigationContext context)
    {
        return Task.CompletedTask;
    }

    public override Task OnNavigatedAsync(NavigationContext context)
    {
        base.OnNavigatedAsync(context); // Keep this. It will automatically assign the navigatable to the CurrentModel property.
        return Task.CompletedTask;
    }
}
```

#### Whats the difference between `OnNavigatingAsync` and `OnNavigatedAsync`?
- `OnNavigatingAsync` is a phase of the navigation where **navigation cancellation is still possible**. `NavigationContext` provides the `Cancel` method that can cancel the navigation.
- `OnNavigatedAsync` is a phase of the navigation where everything is performed and actions cannot be interrupted any further.

For categorizing your navigation host, you can use any unique object in order to identify the group; But using an enum is recommended. *(Ex: `enum NavigationCategory { Default, Settings, etc... }`)*

#### Next, we need to define some navigatables that will be used as pages inside of the `MyNavigationHost` navigation host.
```csharp
public sealed partial class MyNavigatable : ObservableNavigatableBase
{
    public override Task OnNavigatingToAsync(NavigationContext context)
    {
        return base.OnNavigatingToAsync(context);
    }

    public override Task OnNavigatedToAsync(NavigationContext context)
    {
        return base.OnNavigatedToAsync(context);
    }

    public override Task OnNavigatingFromAsync(NavigationContext context)
    {
        return base.OnNavigatingFromAsync(context);
    }

    public override Task OnNavigatedFromAsync(NavigationContext context)
    {
        return base.OnNavigatedFromAsync(context);
    }
}
```

#### Same concept as before:
`OnNavigatingFromAsync/OnNavigatingToAsync` is where you are still able to cancel the navigation, while `OnNavigatedToAsync/OnNavigatedFromAsync` is where it's not possible anymore.
`OnNavigatedFromAsync` can also be very helpful when you need to unsubscribe from events, stop refresh/update timers, or turn off any tasks that shouldn't work while the page is not active.

#### How to navigate
```csharp
// Inside some view-model or a service. For example MyNavigationHost.
private readonly INavigationService _navigationService;

public MyNavigationHost(INavigationService navigationService)
{
    _navigationService = navigationService;
}

public async Task Navigate() 
{
    await _navigationService.NavigateAsync(NavigationOptions.FromNavigationHost(this).ToNavigatable<HomeViewModel>());
}
```

`NavigateAsync` method takes navigation options which is fairly flexible, and can take instances and types of the models you are working with.
Navigation parameters are also possible to provide.

### IMPORTANT NOTE
If your **navigation host** is **not** cached *(`IsCached` property in the `NavigationHostAttribute` is set to false)*, then you should use the **instance** of the navigation host in the navigation options.
Otherwise, the navigation service will demand a service from a `IServiceProvider` while your `navigation host` is transient, thus obtaining a completely new instance and failing the navigation execution.

## How to show popup/dialog windows
First, we need to define a popup that will be the logic part of your window.
Mark the model using the `[Popup<TWindow>]` attribute.
After that, similarly, you can implement the `IPopup<TWindow, TResult>` directly, but using the built-in base class is **highly** recommended here.
`ObservablePopupBase<TWindow, TResult>` - abstract class that implements `IPopup<TWindow, TResult>` and extends the `ObservableObject` class to provide observable properties for bindings.
The base class also provides a `SetResult` method that allows you to easily close up the window and return a desired result.

```csharp
public sealed partial class MyPopup : ObservablePopupBase<MyWindow, string>;
```

We will also need the `Window` to inherit `IPopupWindow`. No implementation is required.

```csharp
public sealed partial class MyWindow : Window, IPopupWindow
{
    ...
}
```

#### Methods
- `SetResult()` - sets the result as successful/intended and closes the dialog.
- `OnPopupOpenedAsync()`/`OnPopupOpenedAsync()` - invoked when the corresponding events occur.
- `GetValidationErrors()` - returns a read-only collection of validation errors within a specified validatable container.

#### How to display
```csharp
public sealed partial class MyViewModel : ObservableObject
{
    private readonly IPopupService _popupService;

    public MyViewModel(IPopupService popupService)
    {
        _popupService = popupService;
    }

    async Task ShowPopup()
    {
        var result = await _popupService.ShowPopupAsync<MyPopup, MyWindow, string>();
        ...   
    }
}
```

## Interceptors
Interceptors are DI objects that can be resolved as a transient or a singleton service.

#### As for the latest version, these services support execution interception:
- `INavigationService` *(Pre/Post)*
- `IPopupService` *(Pre/Post)*

### How to use interceptors
Lets create a navigation service interceptor.
You can implement `IInterceptor<>` interface, but using the built-in base class is recommended.
If you implement an interface with 2 generic arguments, then the methods with no context argument will be ignored during the runtime.

```csharp
[Interceptor]
// Important note: 2nd generic parameter (NavigationContext) is not mandatory, but then your interception will not be able
// to access the service's context.
public sealed class NavigationInterceptor : InterceptorBase<INavigationService, NavigationContext>
{
    public override async Task InterceptPreExecutionAsync(NavigationContext context)
    {
        ...
    }

    public override async Task InterceptPostExecutionAsync(NavigationContext context)
    {
        ...
    }
}
```