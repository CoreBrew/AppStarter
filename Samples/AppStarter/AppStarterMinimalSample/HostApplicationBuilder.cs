using CoreBrew.AppStarter.Builder;

namespace AppStarterMinimalSample;

public class HostApplicationBuilder : CoreBrewHostApplicationBuilder
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddHostedService<TestHostedService>();
    }
}