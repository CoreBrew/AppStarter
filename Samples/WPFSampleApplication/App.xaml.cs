using System.Windows;
using CoreBrew.AppStarter.Builder;
using Microsoft.Extensions.Hosting;

namespace WPFSampleApplication;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var webApp = CoreBrewApplicationHostFactory.Build(new HostApplicationBuilder()).RunAsync();
    }
    
    
}