using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.General;
using PsdFramework.ModularWpf.View.Models;
using System.Windows;

namespace PsdFramework.ModularWpf.Tests.Unit.View;

public sealed class ViewTests
{
    private readonly IServiceProvider _provider;

    public ViewTests()
    {
        _provider = new ServiceCollection()
            .AddComponents(ComponentOptions.Empty().WithConcreteTypes([typeof(MyModel), typeof(MyModelCached)]))
            .BuildServiceProvider();
    }

    [Fact]
    public void GetRequiredService_ShouldReturnModellessAndRegularViews_WhenServicesResolved()
    {
        TestHelper.ExecuteOnStaThread(() =>
        {
            var modellessView = _provider.GetRequiredService<IView<MyView>>();
            var regularView = _provider.GetRequiredService<IView<MyView, MyModel>>();

            modellessView.Should().NotBe(regularView);
            modellessView.View.DataContext.Should().BeOfType<MyModel>();

            regularView.View.DataContext.Should().BeOfType<MyModel>();
            regularView.Model.Should().BeOfType<MyModel>();
        });
    }

    [Fact]
    public void GetRequiredService_ShouldReturnModellessAndRegularViews_ReusingTheSameInstances_WhenServicesResolved()
    {
        TestHelper.ExecuteOnStaThread(() =>
        {
            var modellessView = _provider.GetRequiredService<IView<MyViewCached>>();
            var modellessView2 = _provider.GetRequiredService<IView<MyViewCached>>();
            var regularView = _provider.GetRequiredService<IView<MyViewCached, MyModelCached>>();
            var regularView2 = _provider.GetRequiredService<IView<MyViewCached, MyModelCached>>();

            modellessView.Should().Be(modellessView2);
            modellessView.Should().Be(regularView);
            modellessView.Should().Be(regularView2);
            modellessView.View.DataContext.Should().BeOfType<MyModelCached>();

            regularView.View.DataContext.Should().BeOfType<MyModelCached>();
            regularView.Model.Should().BeOfType<MyModelCached>();
        });
    }


}

[View<MyView>]
file sealed class MyModel;
file sealed class MyView : FrameworkElement;

[View<MyViewCached>(IsCached = true)]
file sealed class MyModelCached;
file sealed class MyViewCached : FrameworkElement;