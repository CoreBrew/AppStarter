using CoreBrew.AppStarter.Builder;
using CoreBrew.AppStarter.Options;

namespace AppStarterWebApplication;

public class AppBuilder : WebCoreBrewHostApplicationBuilder
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddHostedService<TestHostedService>();
    }

    protected override void ConfigureConfiguration(IConfigurationManager configurationManager, CoreBrewOptionsBinder optionsBinder)
    {
        base.ConfigureConfiguration(configurationManager, optionsBinder);
        optionsBinder.AddOptions<TestOption>();
    }
}