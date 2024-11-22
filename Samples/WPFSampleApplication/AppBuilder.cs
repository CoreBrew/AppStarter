using CoreBrew.AppStarter.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WPFSampleApplication;

public class AppBuilder : CoreBrewWebAppBuilder
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddHostedService<TestHostedService>();
    }
}