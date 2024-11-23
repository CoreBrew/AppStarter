using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

/// <summary>
/// Interface to allow calling build
/// </summary>
public interface ICoreBrewHostApplicationBuilder
{
    /// <summary>
    /// Build the application
    /// </summary>
    public IHost Build();
}