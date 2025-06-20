using CoreBrew.AppStarter.HostedService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppStarterWpfSample;

public class TestHostedService : CoreBrewHostedServiceBase
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<CoreBrewHostedServiceBase> _logger;

    public TestHostedService(IHostApplicationLifetime hostApplicationLifetime, ILogger<TestHostedService> logger) :
        base(hostApplicationLifetime, logger)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _hostApplicationLifetime.ApplicationStopping.Register(SlowStoppingCallback);
        _logger = logger;
        TargetCycleTime = TimeSpan.FromSeconds(5);
        ShutdownOnException = false;
    }

    private async void SlowStoppingCallback()
    {
        await Task.Delay(TimeSpan.FromSeconds(8));
    }

    protected override async Task Execute()
    {
        _logger.LogInformation("This is actually not bad");
        Thread.Sleep(8000);
        _ = Task.Run(() => _hostApplicationLifetime.StopApplication());


        //await TestExceptionIsCaughtInCoreBrewHostedServiceBase();
    }

    private Task TestExceptionIsCaughtInCoreBrewHostedServiceBase()
    {
        Thread.Sleep(4000);
        throw new Exception();
    }
}