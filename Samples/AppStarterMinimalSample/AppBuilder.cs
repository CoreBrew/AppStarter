using CoreBrew.AppStarter.Builder;

namespace AppStarterMinimalSample;

public class AppBuilder : CoreBrewWebAppBuilder
{
    public AppBuilder(string[] args) : base(
        new WebApplicationOptions { Args = args })
    {
        
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddHostedService<TestHostedService>();
    }
}