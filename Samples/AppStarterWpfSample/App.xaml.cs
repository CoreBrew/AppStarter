using System.Windows;

namespace AppStarterWpfSample;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        Host = new AppBuilder().Build();
        base.OnStartup(e);
    }
}