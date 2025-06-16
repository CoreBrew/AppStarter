using System.ComponentModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Desktop;

public abstract class CoreBrewApplication<TMainWindow> : Application where TMainWindow : Window, new()
{
    private CancellationToken _applicationStoppingToken;
    private Task _hostTask = null!;
    private CancellationTokenSource _applicationStoppingTokenSource = null!;
    private IHostApplicationLifetime _hostLifeTime = null!;

    protected IHost Host { get; set; } = null!;
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _applicationStoppingTokenSource = new CancellationTokenSource();
        _applicationStoppingToken = _applicationStoppingTokenSource.Token;
        _hostTask = Host.RunAsync(_applicationStoppingToken);
        _hostLifeTime = Host.Services.GetRequiredService<IHostApplicationLifetime>();
        
        MainWindow = new TMainWindow();
        MainWindow.Closing += MainWindowOnClosing;
        MainWindow.Show();
    }

    private async void MainWindowOnClosing(object? sender, CancelEventArgs e)
    {
        await _applicationStoppingTokenSource.CancelAsync();
        await _hostTask.WaitAsync(_applicationStoppingToken);
    }
}