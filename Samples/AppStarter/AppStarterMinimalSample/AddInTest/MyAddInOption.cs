using CoreBrew.AppStarter.Options;

namespace AppStarterMinimalSample.AddInTest;

public class MyAddInOption : OptionsSection
{
    public string MyAddInOptionsString { get; set; } = "Hello World! from MyAddInOption";
}