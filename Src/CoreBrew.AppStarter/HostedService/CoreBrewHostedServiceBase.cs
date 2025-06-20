using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.HostedService;

/// <summary>
/// Base CoreBrew background service with basic quality of life wrapping of the .net BackgroundService
/// Ensures that the execute method is not run before the application started is called.
/// </summary>
public abstract class CoreBrewHostedServiceBase : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<CoreBrewHostedServiceBase> _logger;
    private DateTime _cycleTimeStart;
    private bool _applicationStarted;
    private CancellationToken _stoppingToken;
    private Thread _thread;
    private TaskCompletionSource _taskCompletionSource;

    /// <summary>
    /// The target execute method cycle time. If the execute method takes shorter
    /// than the target a delay will automatically be inserted to force the cycle time.
    /// If the execute method takes longer than the target cycle time it will rerun the execute method immediately
    /// </summary>
    /// <value>defaults to 1 second</value>    
    protected TimeSpan TargetCycleTime { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// <see cref="ShutdownOnException"/> configures if the hosted service should close the application on any unhandled
    /// exception. It defaults to true. If set to false, the hosted service will simply log the exception and continue
    /// to run.
    /// </summary>
    protected bool ShutdownOnException { get; set; }

    /// <summary>
    /// The name of the long-running background worker
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// When true, a dedicated new thread will be assigned to the background service
    /// </summary>
    public bool UseDedicatedThread { get; set; }

    /// <inheritdoc />
    protected CoreBrewHostedServiceBase(IHostApplicationLifetime hostApplicationLifetime,
        ILogger<CoreBrewHostedServiceBase> logger)
    {
        Name = GetType().Name;
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
        _hostApplicationLifetime.ApplicationStarted.Register(HostApplicationStarted);
        _hostApplicationLifetime.ApplicationStopping.Register(HostApplicationStopping);
    }

    private void HostApplicationStopping()
    {
        StopAsync(_stoppingToken);
        _logger.LogInformation("HostedService: {ServiceName} is stopping", GetType().Name);
    }

    /// <inheritdoc />
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _stoppingToken = stoppingToken;
        return UseDedicatedThread ? RunWithThread() : RunWithTask();
    }

    private Task RunWithTask()
    {
        return Task.Run(async void () =>
        {
            try
            {
                while (!_stoppingToken.IsCancellationRequested)
                {
                    _cycleTimeStart = DateTime.UtcNow;
                    if (_applicationStarted)
                    {
                        await InternalExecute();
                    }

                    EnsureTargetCycleTimeHasElapsed(_stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Unhandled exception in HostedService: {ServiceName}", GetType().Name);
            }
        }, _stoppingToken);
    }

    private Task RunWithThread()
    {
        _taskCompletionSource = new TaskCompletionSource();
        _thread = new Thread(
            async void () =>
            {
                try
                {
                    while (!_stoppingToken.IsCancellationRequested)
                    {
                        _cycleTimeStart = DateTime.UtcNow;
                        if (_applicationStarted)
                        {
                            await InternalExecute();
                        }

                        EnsureTargetCycleTimeHasElapsed(_stoppingToken);
                    }
                    _taskCompletionSource.SetResult();
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "Unhandled exception in HostedService: {ServiceName}", GetType().Name);
                    _taskCompletionSource.SetException(e);
                }
            }
        )
        {
            Name = Name,
        };

        _thread.Start();
        
        return _taskCompletionSource.Task;
    }

    private void HostApplicationStarted()
    {
        _applicationStarted = true;
    }

    private async Task InternalExecute()
    {
        try
        {
            await Execute();
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Unhandled exception in Execute method for hosted service: {ClassName}",
                GetType().Name);
            if (ShutdownOnException)
            {
                _logger.LogCritical("Shutting down from hosted service: {ClassName}", GetType().Name);
                _hostApplicationLifetime.StopApplication();
                await StopAsync(_stoppingToken);
            }
            else
            {
                _logger.LogCritical("HostedService: {ClassName} was configured to continue to run on exception",
                    GetType().Name);
            }
        }
    }

    /// <summary>
    /// The Execute method of the CoreBrewHostedService. If any unhandled exceptions escape this method 
    /// </summary>
    /// <returns></returns>
    protected abstract Task Execute();

    private void EnsureTargetCycleTimeHasElapsed(CancellationToken stoppingToken)
    {
        var elapsedTime = DateTime.UtcNow - _cycleTimeStart;
        if (elapsedTime < TargetCycleTime)
        {
            stoppingToken.WaitHandle.WaitOne(TargetCycleTime - elapsedTime);
        }
    }
}