using Microsoft.Extensions.Options;

namespace AppStarterMinimalSample.HostAppExtensionTest;

public class MyServiceClass
{
    private readonly ILogger<MyServiceClass> _logger;
    private TestOption _testOption;

    public MyServiceClass(ILogger<MyServiceClass> logger,IOptionsMonitor<TestOption> optionsMonitor)
    {
        _logger = logger;
        _testOption = optionsMonitor.CurrentValue;
        optionsMonitor.OnChange(option =>
        {
            _logger.LogInformation("Options changed: " + option.MyString);
            _testOption = option;
        });
    }

    public void DoSomething()
    {
        _logger.LogInformation("Doing something");
        _logger.LogInformation(_testOption.MyString);
    }
}