# PsdFramework.ModularWpf

#### `ModularWpf` is based off the CommunityToolkit.Mvvm package, a small framework library that helps with modulation in a WPF application.

## Features
- IServiceCollection extensions that help with registering common models of the MVVM pattern: `AddView<TView, TViewModel>()`, `AddCachedView<TView, TViewModel>()`, `AddViewModel<TViewModel>()`
- Modular Windows - you can assign a `Window` object with a `IModularWindow` interface, then await the window and get the result via the service

## How to use
#### First lets define our window object:
```csharp
public sealed partial class MyModularWindow : Window, IModularWindow
{
	public MyModularWindow()
	{
		InitializeComponent();
	}
}
```

#### Now lets register the window and the service
```csharp
myServiceCollection.AddModularWindow<MyModularWindow>();
myServiceCollection.AddModularWindowsService();

// OR if you dont want to register each window manually:
myServiceCollection.AddModularWindows(assemblyFilter: a => a == typeof(MyModularWindow).Assembly)); // ! automatically registers the service as well !
```

#### Use-case
```csharp
public sealed partial class MyViewModel : ObservableObject
{
	private readonly IModularWindowsService _modularWindowsService;

	public MyViewModel(IModularWindowsService modularWindowsService)
	{
		_modularWindowsService = modularWindowsService;
	}

	[RelayCommand]
	private async Task OpenMyWindowAsync()
	{
		await _modularWindowsService.ShowWindowAsync<MyModularWindow>();
	}

	[RelayCommand]
	private async Task OpenMyWindowWithResultAsync()
	{
		// string is a result type that is defined in the IModularWindow<TResult> interface
		// regular IModularWindow doesnt have a return type, so in the implementation just keep it with the NotImplementedException
		var result = await _modularWindowsService.ShowWindowAsync<MyModularWindow, string>(); 
		Console.WriteLine(result);
	}
}
```