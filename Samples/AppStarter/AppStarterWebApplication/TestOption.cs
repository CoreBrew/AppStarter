using CoreBrew.AppStarter.Options;

namespace AppStarterWebApplication;

public class TestOption : OptionsSection
{
    public string MyString { get; set; }
    public int MyInt { get; set; }
}