using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

public class CoreBrewWebAppBuilder
{
    protected readonly WebApplicationBuilder ApplicationBuilder;

    internal CoreBrewWebAppBuilder(WebApplicationOptions options, Action<IHostBuilder>? configureDefaults = null)
    {
        ApplicationBuilder = WebApplication.CreateBuilder(options);
        ApplicationBuilder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        ApplicationBuilder.Services.AddEndpointsApiExplorer();
        ApplicationBuilder.Services.AddSwaggerGen();

        //can now get the app with build
        //var app = ApplicationBuilder.Build();

                
    }
}