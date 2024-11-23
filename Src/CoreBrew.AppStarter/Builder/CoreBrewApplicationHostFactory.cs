using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

public static class CoreBrewApplicationHostFactory
{
    /// <summary>
    /// Creates the basic web application
    /// </summary>
    /// <param name="hostApplicationBuilder"></param>
    /// <returns></returns>
    public static IHost Build(ICoreBrewHostApplicationBuilder hostApplicationBuilder)
    {
        return hostApplicationBuilder.Build();;
    }
}