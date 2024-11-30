using CoreBrew.AppStarter.HostedService;

namespace SerilogSampleService;

public class TestHostedService : CoreBrewHostedServiceBase
{
    private readonly ILogger<CoreBrewHostedServiceBase> _logger;

    public TestHostedService(IHostApplicationLifetime hostApplicationLifetime, ILogger<TestHostedService> logger) :
        base(hostApplicationLifetime, logger)
    {
        _logger = logger;
        TargetCycleTime = TimeSpan.FromSeconds(5);
        ShutdownOnException = false;
    }

    protected override async Task Execute()
    {
        _logger.LogInformation("This is actually not bad");
    }
}