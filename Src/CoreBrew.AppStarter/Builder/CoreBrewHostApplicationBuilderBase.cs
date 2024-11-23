using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.Builder;

public abstract class CoreBrewHostApplicationBuilderBase
{
    /// <summary>
    /// Build the application
    /// </summary>
    public abstract IHost Build();    
}

public abstract class CoreBrewHostApplicationBuilderBase<T> : CoreBrewHostApplicationBuilderBase where T : IHostApplicationBuilder
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
        PreConfigureInternal();
        ConfigureInternal();
        PostConfigureInternal();
    }

    private void PostConfigureInternal()
    {
        PostConfigure();
    }

    private void PreConfigureInternal()
    {
        PreConfigure();
    }

    private void ConfigureInternal()
    {
        ConfigureServices(Services);
        ConfigureLogging(Services, ApplicationBuilder.Logging);        
        Configure();
    }

    /// <summary>
    /// Marked virtual to allow inheritors to avoid overriding 
    /// </summary>
    protected virtual void PostConfigure(){}
    /// <summary>
    /// Marked virtual to allow inheritors to avoid overriding
    /// </summary>
    protected virtual void Configure(){}
    /// <summary>
    /// Marked virtual to allow inheritors to avoid overriding 
    /// </summary>
    protected virtual void PreConfigure(){}

    /// <summary>
    /// The IOC service collection
    /// </summary>
    public IServiceCollection Services => ApplicationBuilder.Services;
    
    
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
    protected virtual void ConfigureConfiguration(IConfigurationManager configurationManager)
    {
        
    }
}