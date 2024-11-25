using CoreBrew.AppStarter.Options;

namespace AppStarterMinimalSample.HostAppExtensionTest.HostAppExtensionTestLevel2;

public class MyOtherAddInOption : OptionsSection
{
    public string MyOtherAddInOptionsString { get; set; } = "Hello World! from MyOtherAddInOption";
}