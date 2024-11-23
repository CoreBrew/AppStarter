using CoreBrew.AppStarter.Builder;

namespace AppStarterMinimalSample;

public class HostApplicationBuilder : CoreBrewWebHostApplicationBuilder
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddHostedService<TestHostedService>();
    }
}