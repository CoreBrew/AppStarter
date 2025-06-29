using CoreBrew.AppStarter.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.Builder;

/// <summary>
/// Wrapper class for a <see cref="CoreBrewHostApplicationExtension"/> collection
/// </summary>
public class HostApplicationExtensionRegistry
{
    private readonly IHostApplicationBuilder _builder;
    private readonly CoreBrewOptionsBinder _optionsBinder;
    private readonly HashSet<Type> _hostApplicationExtensionTypes = [];

    public HostApplicationExtensionRegistry(IHostApplicationBuilder builder, CoreBrewOptionsBinder optionsBinder)
    {
        _builder = builder;
        _optionsBinder = optionsBinder;
        _hostApplicationExtensionTypes.Add(builder.GetType());
    }

    /// <summary>
    /// Register a HostApplication add in, and ensure its only done once per HostApplication
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Register<T>() where T : CoreBrewHostApplicationExtension, new()
    {
        if (!_hostApplicationExtensionTypes.Add(typeof(T))) return;
        var hostAppExtension = new T
        {
            ApplicationBuilder = _builder,
            OptionsBinder = _optionsBinder
        };
        hostAppExtension.ConfigureHostAppExtension(this);
    }
    
    /// <summary>
    /// Register a HostApplication add in by type, and ensure it's only done once per HostApplication
    /// </summary>
    /// <param name="type">The type of the add-in to register.</param>
    public void Register(Type type)
    {
        if (!typeof(CoreBrewHostApplicationExtension).IsAssignableFrom(type))
            throw new ArgumentException($"Type {type.FullName} must inherit from {nameof(CoreBrewHostApplicationExtension)}.", nameof(type));
    
        if (type.GetConstructor(Type.EmptyTypes) == null)
            throw new ArgumentException($"Type {type.FullName} must have a parameterless constructor.", nameof(type));
    
        // Use reflection to invoke the generic method
        var method = GetType().GetMethod(nameof(Register), Type.EmptyTypes);
        var genericMethod = method?.MakeGenericMethod(type);
        genericMethod?.Invoke(this, null);
    }
}

/// <summary>
/// Interface for the basic calls shared by both the actual builder class
/// and any CoreBrew application add ins 
/// </summary>
public abstract class CoreBrewHostApplicationExtension : IHostApplicationBuilder
{
    /// <inheritdoc />
    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull
    {
        ApplicationBuilder.ConfigureContainer(factory, configure);
    }

    /// <inheritdoc />
    public IConfigurationManager Configuration => ApplicationBuilder.Configuration;

    /// <inheritdoc />
    public IHostEnvironment Environment => ApplicationBuilder.Environment;

    /// <inheritdoc />
    public ILoggingBuilder Logging => ApplicationBuilder.Logging;

    /// <inheritdoc />
    public IMetricsBuilder Metrics => ApplicationBuilder.Metrics;

    /// <inheritdoc />
    public IDictionary<object, object> Properties => ApplicationBuilder.Properties;    
    
    /// <summary>
    /// The OptionsBinder
    /// </summary>
    public CoreBrewOptionsBinder OptionsBinder { get; init; } = null!;


    /// <summary>
    /// The application builder
    /// </summary>
    public IHostApplicationBuilder ApplicationBuilder { get; init; } = null!;

    /// <summary>
    /// The IOC service collection
    /// </summary>
    public IServiceCollection Services => ApplicationBuilder.Services;

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
    /// Add host app extensions to the IOC container. This is where you would add in any custom
    /// extensions to The host application that inherits from <see cref="CoreBrewHostApplicationExtension"/> 
    /// </summary>
    /// <param name="hostApplicationExtensionRegistry"></param>
    protected virtual void AddHostAppExtensions(HostApplicationExtensionRegistry hostApplicationExtensionRegistry)
    {
    }

    /// <summary>
    /// Call the required configuration and option add ins to the IOC container
    /// </summary>
    /// <param name="hostApplicationExtensionRegistry"></param>
    public void ConfigureHostAppExtension(HostApplicationExtensionRegistry hostApplicationExtensionRegistry)
    {
        ConfigureConfiguration(ApplicationBuilder.Configuration, OptionsBinder);
        ConfigureServices(ApplicationBuilder.Services);
        ConfigureLogging(ApplicationBuilder.Services,ApplicationBuilder.Logging);
        AddHostAppExtensions(hostApplicationExtensionRegistry);
    }
    
    /// <summary>
    /// Configures base logging
    /// </summary>
    /// <param name="services"></param>
    /// <param name="loggingBuilder"></param>
    protected virtual void ConfigureLogging(IServiceCollection services, ILoggingBuilder loggingBuilder)
    {
    }    
}

/// <inheritdoc />
public abstract class CoreBrewHostApplicationBuilderBase<TApplication> : CoreBrewHostApplicationExtension where TApplication : class, IHost
{
    private TApplication _app = null!;
    private ILogger<TApplication> _logger = null!;
    private IHostApplicationLifetime _hostApplicationLifetime = null!;
    private IHostEnvironment _hostEnvironment = null!;
    private readonly HostApplicationExtensionRegistry _hostApplicationExtensionRegistry;

    /// <summary>
    /// Construct the most basic app
    /// </summary>
    /// <param name="applicationBuilder"></param>
    protected CoreBrewHostApplicationBuilderBase(IHostApplicationBuilder applicationBuilder)
    {
        ApplicationBuilder = applicationBuilder;
        OptionsBinder = new CoreBrewOptionsBinder(ApplicationBuilder);
        _hostApplicationExtensionRegistry = new HostApplicationExtensionRegistry(applicationBuilder,OptionsBinder);
        Configure();
    }

    private void Configure()
    {
        ConfigureLogging(Services, ApplicationBuilder.Logging);
        ConfigureHostAppExtension(_hostApplicationExtensionRegistry);
    }

    /// <summary>
    /// Build the application
    /// </summary>
    public TApplication Build()
    {
        _app = BuildApp();
        _logger = _app.Services.GetRequiredService<ILogger<TApplication>>();
        _hostApplicationLifetime = _app.Services.GetRequiredService<IHostApplicationLifetime>();
        _hostEnvironment = _app.Services.GetRequiredService<IHostEnvironment>();
        _hostApplicationLifetime.ApplicationStopping.Register(HostApplicationStopping);
        LogApplicationStart(_logger);
        ConfigureApplication(_app);
        return _app;
    }

    /// <summary>
    /// Call the abstract function that inheritors must implement, and build the specific host application type
    /// </summary>
    /// <returns></returns>
    protected abstract TApplication BuildApp();


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
    /// Allow implementers to change the default logging messages at start
    /// </summary>
    protected virtual void LogApplicationStart(ILogger logger)
    {
        logger.LogInformation("Host application built.");
        logger.LogInformation(
            $"Application starting, current user is: {System.Environment.UserName}. Environment name is: {_hostEnvironment.EnvironmentName}. OS Version string is {System.Environment.OSVersion.VersionString}");
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
            $"Host Application stopping, current user is: {System.Environment.UserName}. Environment name is: {_hostEnvironment.EnvironmentName}. OS Version string is {System.Environment.OSVersion.VersionString}");
    }

    /// <summary>
    /// After application has been built. allow to get a hook that allows to call
    /// out any required service classes and other misc actions that needs to be done before calling run/runasync
    /// </summary>
    protected virtual void ConfigureApplication(TApplication app)
    {
    }
}