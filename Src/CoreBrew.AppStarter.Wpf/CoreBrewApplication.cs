using System.ComponentModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
    
    /// <summary>
    /// The concrete WPF application needs to implement this abstract method and create the concrete host application, then
    /// return it as an <see cref="IHostApplicationBuilder"/>. On purpose the CoreBrew Appstarter lib does not provide an interface
    /// or type that simply exposes the build method, as to avoid having third party library integrates call build to early.
    /// Build should be called by the actual implementation application
    /// </summary>
    /// <param name="buildHostApplication">A caller provided <see cref="Func{IHost}"/> that must call the build method
    /// on the underlying implementation of the resolved <see cref="IHostApplicationBuilder"/></param>
    /// <returns><see cref="IHostApplicationBuilder"/> The resolved host application builder interface</returns>
    protected abstract IHostApplicationBuilder ResolveHostApplicationBuilder(out Func<IHost> buildHostApplication);  
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        InitializeHost();
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        _applicationStoppingTokenSource = new CancellationTokenSource();
        _applicationStoppingToken = _applicationStoppingTokenSource.Token;
        _hostLifeTime = Host.Services.GetRequiredService<IHostApplicationLifetime>();
        _hostLifeTime.ApplicationStopping.Register(ApplicationStopping);
        _hostTask = Host.RunAsync(_applicationStoppingToken);
        
        MainWindow = Host.Services.GetRequiredService<TMainWindow>();
        MainWindow.Closing += MainWindowOnClosing;
        MainWindow.Show();
    }

    private void InitializeHost()
    {
        var builder = ResolveHostApplicationBuilder(out var buildHostApplication);
        builder.Services.TryAddSingleton<TMainWindow>();
        Host = buildHostApplication();
    }

    protected virtual void BaseConfigureApplication(IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.TryAddSingleton<TMainWindow>();        
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
            if (MainWindow != null)
            {
                MainWindow.Closing -= MainWindowOnClosing; // prevent recursion
                MainWindow.Close();
            }
            Shutdown();
        }        
        
    }
}