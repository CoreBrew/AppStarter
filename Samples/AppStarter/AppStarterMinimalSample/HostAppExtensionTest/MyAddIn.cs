using AppStarterMinimalSample.HostAppExtensionTest.HostAppExtensionTestLevel2;
using CoreBrew.AppStarter.Builder;
using CoreBrew.AppStarter.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppStarterMinimalSample.HostAppExtensionTest;

public class MyExtension : CoreBrewHostApplicationExtension
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.TryAddSingleton<MyServiceClass>();
    }

    protected override void ConfigureConfiguration(IConfigurationManager configurationManager, CoreBrewOptionsBinder optionsBinder)
    {
        base.ConfigureConfiguration(configurationManager, optionsBinder);
        optionsBinder.AddOptions<MyAddInOption>();
    }

    protected override void AddHostAppExtensions(HostApplicationExtensionRegistry hostApplicationExtensionRegistry)
    {
        base.AddHostAppExtensions(hostApplicationExtensionRegistry);
        hostApplicationExtensionRegistry.Register<MyExtensionAddedExtension>();
    }
}