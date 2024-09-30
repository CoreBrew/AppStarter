using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreBrew.AppStarter.Builder;

public static class CoreBrewApplicationCreator
{
    /// <summary>
    /// Creates the basic web application
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplication CreateWebApplication(CoreBrewWebAppBuilder builder)
    {
        return builder.Build();;
    }
}