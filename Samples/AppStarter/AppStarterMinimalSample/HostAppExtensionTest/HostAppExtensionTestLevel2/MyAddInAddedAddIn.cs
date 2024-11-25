using CoreBrew.AppStarter.Builder;
using CoreBrew.AppStarter.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppStarterMinimalSample.HostAppExtensionTest.HostAppExtensionTestLevel2;

public class MyExtensionAddedExtension : CoreBrewHostApplicationExtension
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.TryAddSingleton<MyOtherServiceClass>();
    }

    protected override void ConfigureConfiguration(IConfigurationManager configurationManager, CoreBrewOptionsBinder optionsBinder)
    {
        base.ConfigureConfiguration(configurationManager, optionsBinder);
        optionsBinder.AddOptions<MyOtherAddInOption>();
    }    
}