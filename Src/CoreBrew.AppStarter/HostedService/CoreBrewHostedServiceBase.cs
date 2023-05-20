using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.HostedService;


/// <summary>
/// Base CoreBrew background service with basic quality of life wrapping of the .net BackgroundService
/// Ensures that the execute method is not run before the application started is called.
/// </summary>
public abstract class CoreBrewHostedServiceBase: BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<CoreBrewHostedServiceBase> _logger;

    /// <summary>
    /// The target execute method cycle time. If the execute method takes shorter
    /// than the target a delay will automatically be inserted to force the cycle time.
    /// If the execute method takes longer than the target cycle time it will rerun the execute method immediately
    /// </summary>
    /// <value>defaults to 1000</value>    
    public int TargetCycleTime { get; set; } = 1000;
    /// <summary>
    /// When execute method ends put this forced delay in
    /// </summary>
    /// <value>defaults to 0</value>
    public int ExecutionMethodEndSleep { get; set; } = 0;

    /// <summary>
    /// Setting one shot to true will result in the execute method being run only once.
    /// <see cref="ExecutionMethodEndSleep"/> will still be respected in this mode
    /// </summary>
    
    public bool OneShot { get; set; } = false;

    protected CoreBrewHostedServiceBase(IHostApplicationLifetime hostApplicationLifetime,ILogger<CoreBrewHostedServiceBase> logger): base()
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
        _hostApplicationLifetime.ApplicationStarted.Register(HostApplicationStarted);
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && !OneShot)
        {
            InternalExecute();
        }

        return Task.CompletedTask;
    }

    private void HostApplicationStarted()
    {
        
    }

    private void InternalExecute()
    {
        try
        {
            Execute();
        }
        catch (Exception e)
        {
            _logger.LogCritical(e,"Unhandled exception in {ClassName}",GetType().Name);
            _hostApplicationLifetime.StopApplication();
        }
    }
    protected abstract void Execute();

}