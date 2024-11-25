using CoreBrew.AppStarter.Options;

namespace AppStarterMinimalSample.AddInTest.AddInFromAddIn;

public class MyOtherAddInOption : OptionsSection
{
    public string MyOtherAddInOptionsString { get; set; } = "Hello World! from MyOtherAddInOption";
}