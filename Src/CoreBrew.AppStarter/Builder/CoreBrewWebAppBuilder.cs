using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.Builder;

public class CoreBrewWebAppBuilder
{
    protected readonly WebApplicationBuilder ApplicationBuilder;
    public IServiceCollection Services => ApplicationBuilder.Services;

    internal CoreBrewWebAppBuilder(WebApplicationOptions options, Action<IHostBuilder>? configureDefaults = null)
    {
        ApplicationBuilder = WebApplication.CreateBuilder(options);

        ApplicationBuilder.Services.AddControllers();
        ApplicationBuilder.Services.AddEndpointsApiExplorer();
        ApplicationBuilder.Services.AddSwaggerGen();

        ConfigureServices(Services);
        ConfigureLogging(Services,ApplicationBuilder.Logging);
        ConfigureConfiguration(ApplicationBuilder.Configuration);
        
    }
    public WebApplication Build()
    {
        return ApplicationBuilder.Build();
    }

    protected void ConfigureLogging(IServiceCollection services,ILoggingBuilder loggingBuilder)
    {
        services.AddLogging();
    }

    protected void ConfigureServices(IServiceCollection services)
    {
        
    }

    protected void ConfigureConfiguration(ConfigurationManager configurationManager)
    {
        
    }
}