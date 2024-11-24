using CoreBrew.AppStarter.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.Builder;

/// <summary>
/// Handles the most basic stuff for HostApplication building. But must still rely on having the <see cref="BuildApp"/>
/// function called from the generic class <see cref="CoreBrewHostApplicationBuilderBase{T}"/>
/// </summary>
public abstract class CoreBrewHostApplicationBuilderBase
{
    private IHost _app = null!;
    private ILogger<IHost> _logger = null!;
    private IHostApplicationLifetime _hostApplicationLifetime = null!;
    private IHostEnvironment _hostEnvironment = null!;

    /// <summary>
    /// Call the abstract function that inheritors must implement, and build the specific host application type
    /// </summary>
    /// <returns></returns>
    protected abstract IHost BuildApp();

    /// <summary>
    /// Build the application
    /// </summary>
    public IHost Build()
    {
        _app = BuildApp();
        _logger = _app.Services.GetRequiredService<ILogger<IHost>>();
        _hostApplicationLifetime = _app.Services.GetRequiredService<IHostApplicationLifetime>();
        _hostEnvironment = _app.Services.GetRequiredService<IHostEnvironment>();

        LogApplicationStart(_logger);

        _hostApplicationLifetime.ApplicationStopping.Register(HostApplicationStopping);

        OnAfterBuilt();
        return _app;
    }

    /// <summary>
    /// Allow implementers to change the default logging messages at start
    /// </summary>
    protected virtual void LogApplicationStart(ILogger logger)
    {
        logger.LogInformation("Host application built.");
        logger.LogInformation(
            $"Application starting, current user is: {Environment.UserName}. Environment name is: {_hostEnvironment.EnvironmentName}. OS Version string is {Environment.OSVersion.VersionString}");
    }

    private void HostApplicationStopping()
    {
        LogApplicationStopping(_logger);
    }

    /// <summary>
    /// Allow implementers to change default application stopping log message
    /// </summary>
    /// <param name="logger"></param>
    protected virtual void LogApplicationStopping(ILogger logger)
    {
        logger.LogInformation(
            $"Host Application stopping, current user is: {Environment.UserName}. Environment name is: {_hostEnvironment.EnvironmentName}. OS Version string is {Environment.OSVersion.VersionString}");
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