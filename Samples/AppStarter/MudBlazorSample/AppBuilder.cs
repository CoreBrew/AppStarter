using CoreBrew.AppStarter.Builder;
using MudBlazor.Services;
using MudBlazorSample.Components;

namespace MudBlazorSample;

public class AppBuilder : CoreBrewBlazorHostApplicationBuilder
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        // Add MudBlazor services
        services.AddMudServices();

        // Add services to the container.
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
    }

    protected override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
    }
}