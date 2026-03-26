using System.Windows;
using Microsoft.Extensions.Hosting;

namespace AppStarterWpfSample;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override IHostApplicationBuilder ResolveHostApplicationBuilder(out Func<IHost> buildHostApplication)
    {
        var appBuilder = new AppBuilder();
        buildHostApplication = () => appBuilder.Build();
        return appBuilder;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var appBuilder = new AppBuilder();
        BaseConfigureApplication(appBuilder);
        Host = appBuilder.Build();
        base.OnStartup(e);
    }
}