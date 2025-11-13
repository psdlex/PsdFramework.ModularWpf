# PsdFramework.ModularWpf

#### `ModularWpf` is based on the `CommunityToolkit.Mvvm` package. A framework library that helps with modular design in a WPF application.

## Features
- Scalable and convenient navigation model for navigating between views using view-models.
- Abstract support for dialog windows with view-models. (Keeps the Mvvm pattern satisfied)
- Convenient base classes for view-models and views.
- Automatic View-ViewModel binder.
- Everything is built on top of `Microsoft.Extensions.DependencyInjection` for view-models which makes it easy to integrate into existing applications using this DI framework.
- Easy-to-use app-level exception handler.
- Very simple logger for minimal tasks.

## How to setup your code-base
For the complete experience, inherit the `IModularApplication` interface in your `App.xaml.cs` file.
This way you can access the extension methods provided by the framework.

### App.xaml.cs
```csharp
public sealed partial class App : WinApp, IModularApplication
{
    public IServiceProvider? ServiceProvider { get; private set; } // IModularApplication implementation

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        this.AddGlobalExceptionHandler(); // extension method for IModularApplication

        IServiceCollection services = new ServiceCollection();
        services.AddComponentModels(/* optional assembly filtering predicate: assembly => ... */); // the step that includes the components

        ServiceProvider = services.BuildServiceProvider();
    }
}
```

## How to setup the MainWindow as an Mvvm model
We need to remove the `StartupUri` from the `App.xaml` file and register the `MainViewModel` as a component model implementing `IViewComponentModel<MainWindow>` instead.

### App.xaml
```xml
<Application
    StartupUri="..." // REMOVE THIS LINE
```

### MainViewModel.cs
```csharp
public sealed partial class MainViewModel : ObservableObject, IViewComponentModel<MainWindow>;
```

### App.xaml.cs
```csharp
protected override void OnStartup()
{
    ...

    var mainView = ServiceProvider.GetRequiredService<IView<MainWindow>>();

    MainWindow = mainView.View;
    MainWindow.Show();
}
```

`IView` contains `TView View { get; }` property where `TView` is a `FrameworkElement`.
If you want to access `IView`'s view-model as well, then use the `IView<TView, TViewModel>` interface. (Ex: IView<MainWindow, MainViewModel>().Model)

### Types of components
- `INavigationComponentModel` - component that marks a view-model as a navigation base. A model that stores navigation models inside of it.
- `INavigatableComponentModel` - component that marks a view-model as navigatable. A model that can be navigated to.
- `IPopupComponentModel` - component that marks a view-model as a popup/dialog. A model that can be shown as a dialog window. (not the view itself, but the view's model)
- `IViewComponentModel` - component that marks a view-model as a view for a specific view type to be obtained by the `IView` service.

### Types of services
- `INavigatorService` - service that manages the navigations for `navigation models` via `navigatable models`.
- `IPopupWindowService` - service that manages the showing of dialog windows via `popup models`.

## How to setup navigations
First of all we need to define a navigation model that will hold the navigatable models. We will use `MainViewModel` as for our example.
You can use `INavigationComponentModel` directly, but there is ton of boilerplate to write, so lets use a built-in base class instead.
`ObservableNavigationComponentModel` - abstract class that implements `INavigationComponentModel` and extends the `ObservableObject` class to provide observable properties for binding.

```csharp
[NavigationComponentModel(category: (object)"some category")]
public sealed partial class MainViewModel : ObservableNavigationComponentModel
{
    public override Task OnNavigating(NavigationContext context)
    {
        return Task.CompletedTask;
    }

    public override Task OnNavigated(NavigationContext context)
    {
        base.OnNavigated(context); // KEEP THIS
        return Task.CompletedTask;
    }
}
```

Whats the difference between `OnNavigating` and `OnNavigated`?
- `OnNavigating` is a stage of navigation where **navigation cancellation is still possible**. `NavigationContext` provides a method `Cancel()` that can cancel the navigation.
- `OnNavigated` is a stage where everything is performed and actions cannot be interrupted anymore.

`NavigationComponentModel` is required in order to differentiate between multiple navigation models if there are any. The `category` parameter is used as an identifier for the navigation group.
You can use any object in order to identify the group, but using an enum is recommended. (Ex: `enum NavigationCategory { Default, Settings, etc... }`)

#### Next, we need to define some navigatable models that will be used as pages inside of the `MainViewModel` navigation model.
```
public sealed partial class HomeViewModel : ObservableNavigatableComponentModel
{
    public override Task OnNavigatedTo(NavigationContext context)
    {
        return base.OnNavigatedTo(context);
    }

    public override Task OnNavigatingFrom(NavigationContext context)
    {
        return base.OnNavigatingFrom(context);
    }
}
```

I think you already differentiate methods that end with 'ed' and 'ing'.
`OnNavigatingFrom` is still able to cancel the navigation, while `OnNavigatedTo` is not.
`OnNavigatingFrom` can also be very helpful when you need to unsubscribe from events, stop some timers, or any other turn off any tasks that shouldn't work while the page is not active.

#### How to navigate
```csharp
// App.xaml.cs
services.AddNavigator();

// Inside some view-model. For example MainViewModel
public MainViewModel(INavigatorService navigator)
{
    _navigator = navigator;
}

async Task Navigate() 
{
    await _navigator.NavigateTo<MainViewModel, HomeViewModel>(/* can pass parameters */);
}
```

`.NavigateTo` has **18** overloads for all kinds of cases. You can also pass parameters, specify navigation categories, and more.

### VERY IMPORTANT NOTE
If your `navigation model` is **not** cached *(`IsCached` property in the `Attribute`)*, then you **must** use `navigation model` **instance** in the navigation call.
Otherwise the navigator will get demand a service from a `IServiceCollection` while your `navigation model` is transient.. you understand what will happen, right?
It will grab a completely new instance and your navigation will fail.

## How to setup popup/dialog windows
First of all we need to define a popup model that will be used as a dialog window.
```csharp
public sealed class MyPopupViewModel : ObservablePopupComponentModel<MyPopupWindow, MyPopupResult>
{
    public override Task OnPopupOpened(ContextualParameters? parameters)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnPopupClosed()
    {
        return base.OnPopupClosed();
    }
}
```

`MyPopupResult` is a type that will be used as the result of the dialog window. You can use `object`/`object?` if you don't need any specific result type.

- `SetResult()` - sets the result as successful/intended and closes the dialog.
- `OnPopupOpened()`/`OnPopupClosed()` - pretty obvious
- `ContextualParameters` are passed parameters when opening the dialog.

## How to show popup/dialog windows
```csharp
// App.xaml.cs
services.AddPopupWindowService();

// Any viewmodel. MainViewModel for our case
public MainViewModel(IPopupWindowService popupService)
{
    _popupService = popupService;
}

async Task ShowPopup()
{
    MyPopupResult result = await _popupService.ShowPopupAsync<MainViewModel, MyPopupViewModel, MyPopupResult>(/* can pass parameters & options */);
}
```