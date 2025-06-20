using System.ComponentModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Wpf;

public abstract class CoreBrewApplication<TMainWindow> : Application where TMainWindow : Window, new()
{
    private CancellationToken _applicationStoppingToken;
    private Task _hostTask = null!;
    private CancellationTokenSource _applicationStoppingTokenSource = null!;
    private IHostApplicationLifetime _hostLifeTime = null!;

    protected IHost Host { get; set; } = null!;
    public IServiceProvider Services => Host.Services;
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        _applicationStoppingTokenSource = new CancellationTokenSource();
        _applicationStoppingToken = _applicationStoppingTokenSource.Token;
        _hostLifeTime = Host.Services.GetRequiredService<IHostApplicationLifetime>();
        _hostLifeTime.ApplicationStopping.Register(ApplicationStopping);
        _hostTask = Host.RunAsync(_applicationStoppingToken);
        
        
        MainWindow = new TMainWindow();
        MainWindow.Closing += MainWindowOnClosing;
        MainWindow.Show();
    }

    private void ApplicationStopping()
    {
        Dispatcher.Invoke(() =>
        {
            if (MainWindow != null)
            {
                MainWindow.Close(); // triggers MainWindowOnClosing
            }
            else
            {
                Shutdown();
            }
        });
    }
    
    private bool _isShuttingDown;
    private async void MainWindowOnClosing(object? sender, CancelEventArgs e)
    {
        if (_isShuttingDown)
            return;

        e.Cancel = true; // Prevent WPF from closing the window
        _isShuttingDown = true;

        try
        {
            await _applicationStoppingTokenSource.CancelAsync();

            try
            {
                await _hostTask.WaitAsync(TimeSpan.FromSeconds(10));
            }
            catch (TimeoutException)
            {
                // Log or handle timeout
            }
        }
        finally
        {
            // Now it's safe to close the window for real
            MainWindow.Closing -= MainWindowOnClosing; // prevent recursion
            MainWindow.Close();
            Shutdown();
        }        
        
    }
}