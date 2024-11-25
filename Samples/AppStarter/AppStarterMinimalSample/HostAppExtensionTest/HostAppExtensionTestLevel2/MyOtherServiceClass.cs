namespace AppStarterMinimalSample.HostAppExtensionTest.HostAppExtensionTestLevel2;

public class MyOtherServiceClass
{
    private readonly ILogger<MyOtherServiceClass> _logger;

    public MyOtherServiceClass(ILogger<MyOtherServiceClass> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation("################## Doing something from other add-in #####################");
    }
}