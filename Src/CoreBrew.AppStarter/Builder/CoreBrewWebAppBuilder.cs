using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.Builder;

/// <summary>
/// Abstract base class setting up the fundamentals of the WebApp
/// </summary>
public abstract class CoreBrewWebAppBuilder
{
    /// <summary>
    /// The application builder
    /// </summary>
    protected readonly WebApplicationBuilder ApplicationBuilder;
    
    /// <summary>
    /// The IOC service collection
    /// </summary>
    public IServiceCollection Services => ApplicationBuilder.Services;

    /// <summary>
    /// 
    /// </summary>
    protected CoreBrewWebAppBuilder()
    {
        ApplicationBuilder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

        ApplicationBuilder.Services.AddControllers();
        ApplicationBuilder.Services.AddEndpointsApiExplorer();
        ApplicationBuilder.Services.AddSwaggerGen();

        Configure();
        return;

        void Configure()
        {
            ConfigureServices(Services);
            ConfigureLogging(Services, ApplicationBuilder.Logging);
            ConfigureConfiguration(ApplicationBuilder.Configuration);
        }
    }
    internal WebApplication Build()
    {
        var app = ApplicationBuilder.Build();
        ConfigureApp(app);
        return app;
    }

    /// <summary>
    /// Configure app, override to customize app configuration
    /// </summary>
    /// <param name="app"></param>
    protected virtual void ConfigureApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();             
    }
    
    /// <summary>
    /// Configures base logging
    /// </summary>
    /// <param name="services"></param>
    /// <param name="loggingBuilder"></param>
    protected virtual void ConfigureLogging(IServiceCollection services,ILoggingBuilder loggingBuilder)
    {
        services.AddLogging();
    }

    /// <summary>
    /// Configures services
    /// </summary>
    /// <param name="services"></param>
    protected virtual void ConfigureServices(IServiceCollection services)
    {
        
    }

    /// <summary>
    /// Configure the configuration/options
    /// </summary>
    /// <param name="configurationManager"></param>
    protected virtual void ConfigureConfiguration(ConfigurationManager configurationManager)
    {
        
    }
}