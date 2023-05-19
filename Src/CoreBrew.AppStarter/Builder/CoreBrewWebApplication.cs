namespace CoreBrew.AppStarter.Builder;

public class CoreBrewWebApplication
{
    public static CoreBrewWebAppBuilder CreateBuilder(string[] args) =>
        new(new() { Args = args });    
}