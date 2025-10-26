using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;
using PsdFramework.ModularWpf.Parameters;

namespace PsdFramework.ModularWpf.Navigations.Service;

public interface INavigatorService
{
    // generic navigation
    Task NavigateTo<TNavigation, TNavigatable>()
        where TNavigation : INavigationComponentModel
        where TNavigatable : INavigatableComponentModel;
    Task NavigateTo<TNavigation, TNavigatable>(Action<ParameterBuilder<TNavigatable>> configureParameters)
        where TNavigation : INavigationComponentModel
        where TNavigatable : INavigatableComponentModel;

    Task NavigateTo<TNavigation>(Type navigatableType)
        where TNavigation : INavigationComponentModel;
    Task NavigateTo<TNavigation>(Type navigatableType, Action<ParameterBuilder> configureParameters)
        where TNavigation : INavigationComponentModel;

    Task NavigateTo<TNavigation>(INavigatableComponentModel navigatable)
        where TNavigation : INavigationComponentModel;
    Task NavigateTo<TNavigation>(INavigatableComponentModel navigatable, Action<ParameterBuilder> configureParameters)
        where TNavigation : INavigationComponentModel;


    // category navigation
    Task NavigateTo<TNavigatable>(object category)
        where TNavigatable : INavigatableComponentModel;
    Task NavigateTo<TNavigatable>(object category, Action<ParameterBuilder<TNavigatable>> configureParameters)
        where TNavigatable : INavigatableComponentModel;

    Task NavigateTo(object category, Type navigatableType);
    Task NavigateTo(object category, Type navigatableType, Action<ParameterBuilder> configureParameters);

    Task NavigateTo(object category, INavigatableComponentModel navigatable);
    Task NavigateTo(object category, INavigatableComponentModel navigatable, Action<ParameterBuilder> configureParameters);


    // instance navigation
    Task NavigateTo<TNavigatable>(INavigationComponentModel navigation)
        where TNavigatable : INavigatableComponentModel;
    Task NavigateTo<TNavigatable>(INavigationComponentModel navigation, Action<ParameterBuilder<TNavigatable>> configureParameters)
        where TNavigatable : INavigatableComponentModel;

    Task NavigateTo(INavigationComponentModel navigation, Type navigatableType);
    Task NavigateTo(INavigationComponentModel navigation, Type navigatableType, Action<ParameterBuilder> configureParameters);

    Task NavigateTo(INavigationComponentModel navigation, INavigatableComponentModel navigatable);
    Task NavigateTo(INavigationComponentModel navigation, INavigatableComponentModel navigatable, Action<ParameterBuilder> configureParameters);


    // other
    IEnumerable<INavigatableComponentModel> GetAllNavigatables(object category);
    IEnumerable<INavigatableComponentModel> GetAllNavigatables(object category, Action<ParameterBuilder> configureParameters);
}