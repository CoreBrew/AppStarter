using CoreBrew.AppStarter.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.Builder;

/// <summary>
/// Wrapper class for a <see cref="CoreBrewHostApplicationAddIn"/> collection
/// </summary>
public class AddInCollection
{
    private readonly List<CoreBrewHostApplicationAddIn> _addIns = [];

    /// <summary>
    /// Adds an addin
    /// </summary>
    /// <param name="addIn"></param>
    public void Add(CoreBrewHostApplicationAddIn addIn)
    {
        _addIns.Add(addIn);
    }

    /// <summary>
    /// Call the configure add in for all add ins
    /// </summary>
    /// <param name="optionsBinder"></param>
    public void ConfigureAddIns(CoreBrewOptionsBinder optionsBinder)
    {
        foreach (var addIn in _addIns)
        {
            addIn.ConfigureAddIn(optionsBinder, this);
        }
    }
}

/// <summary>
/// Interface for the basic calls shared by both the actual builder class
/// and any CoreBrew application add ins 
/// </summary>
public abstract class CoreBrewHostApplicationAddIn
{
    /// <summary>
    /// DI the <see cref="IHostApplicationBuilder"/>
    /// </summary>
    /// <param name="applicationBuilder"></param>
    protected CoreBrewHostApplicationAddIn(IHostApplicationBuilder applicationBuilder)
    {
        ApplicationBuilder = applicationBuilder;
    }

    /// <summary>
    /// The application builder
    /// </summary>
    protected IHostApplicationBuilder ApplicationBuilder { get; init; }

    /// <summary>
    /// The IOC service collection
    /// </summary>
    protected IServiceCollection Services => ApplicationBuilder.Services;

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

    /// <summary>
    /// Add AddIns to the IOC container 
    /// </summary>
    protected virtual void AddAddIns(AddInCollection addIns, IHostApplicationBuilder builder)
    {
    }

    /// <summary>
    /// Call the required configuration and option add ins to the IOC container
    /// </summary>
    /// <param name="optionsBinder"></param>
    /// <param name="addInCollection"></param>
    public void ConfigureAddIn(CoreBrewOptionsBinder optionsBinder, AddInCollection addInCollection)
    {
        ConfigureConfiguration(ApplicationBuilder.Configuration, optionsBinder);
        ConfigureServices(ApplicationBuilder.Services);
        AddAddIns(addInCollection,ApplicationBuilder);
    }
}

public abstract class CoreBrewHostApplicationBuilderBase : CoreBrewHostApplicationAddIn
{
    private IHost _app = null!;
    private ILogger<IHost> _logger = null!;
    private IHostApplicationLifetime _hostApplicationLifetime = null!;
    private IHostEnvironment _hostEnvironment = null!;
    private readonly AddInCollection _addIns = new();
    private readonly CoreBrewOptionsBinder _coreBrewOptionsBinder;

    /// <summary>
    /// Construct the most basic app
    /// </summary>
    /// <param name="applicationBuilder"></param>
    protected CoreBrewHostApplicationBuilderBase(IHostApplicationBuilder applicationBuilder) : base(applicationBuilder)
    {
        ApplicationBuilder = applicationBuilder;
        _coreBrewOptionsBinder = new CoreBrewOptionsBinder(ApplicationBuilder);
        Configure();
    }

    private void Configure()
    {
        ConfigureLogging(Services, ApplicationBuilder.Logging);
        ConfigureAddIn(_coreBrewOptionsBinder, _addIns);
        _addIns.ConfigureAddIns(_coreBrewOptionsBinder);
    }

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
    /// Call the abstract function that inheritors must implement, and build the specific host application type
    /// </summary>
    /// <returns></returns>
    protected abstract IHost BuildApp();


    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
    }

    /// <inheritdoc />
    protected override void ConfigureConfiguration(IConfigurationManager configurationManager,
        CoreBrewOptionsBinder optionsBinder)
    {
    }

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
    /// After application has been built. allow to get a hook that allows to call
    /// out any required service classes and other misc actions that needs to be done before calling run/runasync
    /// </summary>
    protected virtual void OnAfterBuilt()
    {
    }
}