namespace AppStarterMinimalSample.HostAppExtensionTest;

public class MyServiceClass
{
    private readonly ILogger<MyServiceClass> _logger;

    public MyServiceClass(ILogger<MyServiceClass> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation("Doing something");
    }
}