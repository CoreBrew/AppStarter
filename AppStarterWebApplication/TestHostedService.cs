using CoreBrew.AppStarter.HostedService;
using Microsoft.Extensions.Options;

namespace AppStarterWebApplication;

public class TestHostedService : CoreBrewHostedServiceBase
{
    private readonly ILogger<CoreBrewHostedServiceBase> _logger;

    public TestHostedService(IHostApplicationLifetime hostApplicationLifetime, ILogger<TestHostedService> logger
    ,IOptions<TestOption> testOption) :
        base(hostApplicationLifetime, logger)
    {
        _logger = logger;
        TargetCycleTime = TimeSpan.FromSeconds(5);
        ShutdownOnException = false;
        if (testOption.Value.MyInt == 1)
        {
            var myInt = testOption.Value.MyInt;
            var myString = testOption.Value.MyString;
        }
    }

    protected override async Task Execute()
    {
        _logger.LogInformation("This is actually not bad");
        await TestExceptionIsCaughtInCoreBrewHostedServiceBase();
    }

    private Task TestExceptionIsCaughtInCoreBrewHostedServiceBase()
    {
        Thread.Sleep(4000);
        throw new Exception();
    }
}