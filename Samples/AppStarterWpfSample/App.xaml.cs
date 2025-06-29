using System.Windows;

namespace AppStarterWpfSample;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var appBuilder = new AppBuilder();
        BaseConfigureApplication(appBuilder);
        Host = appBuilder.Build();
        base.OnStartup(e);
    }
}