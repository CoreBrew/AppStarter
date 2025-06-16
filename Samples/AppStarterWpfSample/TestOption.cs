using CoreBrew.AppStarter.Options;

namespace AppStarterWpfSample;

public class TestOption : OptionsSection
{
    public string MyString { get; set; } = "Hello";
    public int MyInt { get; set; } = 123;
}