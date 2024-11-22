using System.Configuration;
using System.Data;
using System.Windows;
using CoreBrew.AppStarter.Builder;

namespace WPFSampleApplication;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow = new MainWindow();
        MainWindow.Show();        
        CoreBrewApplicationCreator.CreateWebApplication(new AppBuilder()).RunAsync().GetAwaiter().GetResult();

    }
    
    
}