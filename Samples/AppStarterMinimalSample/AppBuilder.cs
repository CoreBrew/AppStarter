using CoreBrew.AppStarter.Builder;

namespace AppStarterMinimalSample;

public class AppBuilder : CoreBrewWebAppBuilder
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddHostedService<TestHostedService>();
    }
}