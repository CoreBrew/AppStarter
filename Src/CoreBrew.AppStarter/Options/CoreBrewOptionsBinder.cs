using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Options;

/// <summary>
/// Simple options binder helper
/// </summary>
public class CoreBrewOptionsBinder(IHostApplicationBuilder applicationBuilder)
{
    /// <summary>
    /// Add options section and bind ti
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public CoreBrewOptionsBinder AddOptions<TOptions>() where TOptions : OptionsSection
    {
        var servicesCollection = applicationBuilder.Services;
        var configurationManager = applicationBuilder.Configuration;
        var optionsInstance = Activator.CreateInstance<TOptions>();
        servicesCollection
            .AddOptions<TOptions>()
            .Bind(configurationManager.GetSection(optionsInstance.SectionName));
        return this;
    }
}