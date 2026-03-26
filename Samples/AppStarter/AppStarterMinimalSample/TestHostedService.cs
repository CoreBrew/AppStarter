using AppStarterMinimalSample.HostAppExtensionTest;
using AppStarterMinimalSample.HostAppExtensionTest.HostAppExtensionTestLevel2;
using CoreBrew.AppStarter.HostedService;
using CoreBrew.AppStarter.Options;

namespace AppStarterMinimalSample;

public class TestHostedService : CoreBrewHostedServiceBase
{
    private int _counter = 0;
    private readonly ILogger<CoreBrewHostedServiceBase> _logger;
    private readonly CoreBrewOptionsStore _optionsStore;

    public TestHostedService(IHostApplicationLifetime hostApplicationLifetime, ILogger<TestHostedService> logger,
        MyServiceClass myServiceClass, MyOtherServiceClass myOtherServiceClass, CoreBrewOptionsStore optionsStore) :
        base(hostApplicationLifetime, logger)
    {
        _logger = logger;
        _optionsStore = optionsStore;
        TargetCycleTime = TimeSpan.FromSeconds(5);
        ShutdownOnException = false;
        myServiceClass.DoSomething();
        myOtherServiceClass.DoSomething();
    }

    protected override async Task Execute()
    {
        _logger.LogInformation("This is actually not bad");
        var options = await _optionsStore.LoadAsync<TestOption>();
        options.MyString = "Hello: " + _counter++;
        await _optionsStore.SaveAsync(options);
        await TestExceptionIsCaughtInCoreBrewHostedServiceBase();
    }

    private Task TestExceptionIsCaughtInCoreBrewHostedServiceBase()
    {
        Thread.Sleep(4000);
        throw new Exception();
    }
}