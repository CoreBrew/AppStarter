using CoreBrew.AppStarter.HostedService;

namespace AppStarterMinimalSample;

public class TestHostedService : CoreBrewHostedServiceBase
{
    private readonly ILogger<CoreBrewHostedServiceBase> _logger;

    public TestHostedService(IHostApplicationLifetime hostApplicationLifetime, ILogger<TestHostedService> logger) :
        base(hostApplicationLifetime, logger)
    {
        _logger = logger;
    }

    protected override void Execute()
    {
        _logger.LogInformation("This is actually not bad");
    }
}