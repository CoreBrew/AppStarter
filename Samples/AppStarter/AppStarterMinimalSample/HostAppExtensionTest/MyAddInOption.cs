using CoreBrew.AppStarter.Options;

namespace AppStarterMinimalSample.HostAppExtensionTest;

public class MyAddInOption : OptionsSection
{
    public string MyAddInOptionsString { get; set; } = "Hello World! from MyAddInOption";
}