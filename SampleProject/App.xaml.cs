using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PsdFramework.ModularWpf.General;
using PsdFramework.ModularWpf.View.Models;
using System.Windows;

namespace SampleProject;

public sealed partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var builder = Host.CreateEmptyApplicationBuilder(null);
        builder.Services.AddComponents();

        var app = builder.Build();

        MainWindow = app.Services.GetRequiredService<IView<MainWindow>>().View;
        MainWindow.Show();
    }
}
