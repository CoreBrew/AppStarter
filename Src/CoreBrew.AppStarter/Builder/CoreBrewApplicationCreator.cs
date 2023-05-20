using Microsoft.AspNetCore.Builder;

namespace CoreBrew.AppStarter.Builder;

public class CoreBrewApplicationCreator
{
    public static WebApplication CreateWebApplication(string [] args)
    {
        CoreBrewWebAppBuilder builder = new(new WebApplicationOptions() { Args = args});
        return builder.Build();
    }
}