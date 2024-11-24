using CoreBrew.AppStarter.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.Builder;

public abstract class CoreBrewHostApplicationBuilderBase
{
    protected abstract IHost InternalBuild();    
    /// <summary>
    /// Build the application
    /// </summary>
    public IHost Build()
    {
        var app = InternalBuild();
        var logger = app.Services.GetRequiredService<ILogger<IHost>>();
        logger.LogInformation("Host application built.");
        logger.LogInformation($"Application starting, current user is: {Environment.UserName}");

        OnAfterBuilt();
        return app;
    }

    /// <summary>
    /// After application has been built. allow
    /// </summary>
    public virtual void OnAfterBuilt()
    {
    }
}

public abstract class CoreBrewHostApplicationBuilderBase<T> : CoreBrewHostApplicationBuilderBase
    where T : IHostApplicationBuilder
{
    /// <summary>
    /// The application builder
    /// </summary>
    protected readonly T ApplicationBuilder;

    /// <summary>
    /// Construct the most basic app
    /// </summary>
    /// <param name="applicationBuilder"></param>
    protected CoreBrewHostApplicationBuilderBase(T applicationBuilder)
    {
        ApplicationBuilder = applicationBuilder;
        Configure();
    }

    private void Configure()
    {
        ConfigureServices(Services);
        ConfigureLogging(Services, ApplicationBuilder.Logging);
        ConfigureConfiguration(ApplicationBuilder.Configuration, new CoreBrewOptionsBinder(ApplicationBuilder));
    }


    /// <summary>
    /// The IOC service collection
    /// </summary>
    public IServiceCollection Services => ApplicationBuilder.Services;


    /// <summary>
    /// Configures base logging
    /// </summary>
    /// <param name="services"></param>
    /// <param name="loggingBuilder"></param>
    protected virtual void ConfigureLogging(IServiceCollection services, ILoggingBuilder loggingBuilder)
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
    /// <param name="optionsBinder"></param>
    protected virtual void ConfigureConfiguration(IConfigurationManager configurationManager,
        CoreBrewOptionsBinder optionsBinder)
    {
    }
}