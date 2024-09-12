using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreBrew.AppStarter.Builder;

public static class CoreBrewApplicationCreator
{
    public static WebApplication CreateWebApplication(CoreBrewWebAppBuilder builder)
    {
        return builder.Build();;
    }
}