using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

public class CoreBrewWebAppBuilder
{
    protected readonly WebApplicationBuilder ApplicationBuilder;

    internal CoreBrewWebAppBuilder(WebApplicationOptions options, Action<IHostBuilder>? configureDefaults = null)
    {
        ApplicationBuilder = WebApplication.CreateBuilder(options);
    }
}