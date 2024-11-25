using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

public static class CoreBrewApplicationHostFactory
{
    /// <summary>
    /// Creates the basic web application
    /// </summary>
    /// <param name="coreBrewHostApplicationBuilder"></param>
    /// <returns></returns>
    public static IHost Build(CoreBrewHostApplicationBuilderBase coreBrewHostApplicationBuilder)
    {
        var host = coreBrewHostApplicationBuilder.Build();
        return host;
    }
}