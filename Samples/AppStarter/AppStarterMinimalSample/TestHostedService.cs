using AppStarterMinimalSample.AddInTest;
using AppStarterMinimalSample.AddInTest.AddInFromAddIn;
using CoreBrew.AppStarter.HostedService;

namespace AppStarterMinimalSample;

public class TestHostedService : CoreBrewHostedServiceBase
{
    private readonly ILogger<CoreBrewHostedServiceBase> _logger;

    public TestHostedService(IHostApplicationLifetime hostApplicationLifetime, ILogger<TestHostedService> logger,
        MyServiceClass myServiceClass, MyOtherServiceClass myOtherServiceClass) :
        base(hostApplicationLifetime, logger)
    {
        _logger = logger;
        TargetCycleTime = TimeSpan.FromSeconds(5);
        ShutdownOnException = false;
        myServiceClass.DoSomething();
        myOtherServiceClass.DoSomething();
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