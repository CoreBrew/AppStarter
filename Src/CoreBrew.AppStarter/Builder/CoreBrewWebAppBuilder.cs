using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBrew.AppStarter.Builder;

public class CoreBrewWebAppBuilder
{
    protected readonly WebApplicationBuilder ApplicationBuilder;
    public IServiceCollection Services => ApplicationBuilder.Services;

    protected CoreBrewWebAppBuilder(WebApplicationOptions options, Action<IHostBuilder>? configureDefaults = null)
    {
        string[] args = new [] {""};
        ApplicationBuilder = WebApplication.CreateBuilder(args);

        ApplicationBuilder.Services.AddControllers();
        ApplicationBuilder.Services.AddEndpointsApiExplorer();
        ApplicationBuilder.Services.AddSwaggerGen();

        ConfigureServices(Services);
        ConfigureLogging(Services,ApplicationBuilder.Logging);
        ConfigureConfiguration(ApplicationBuilder.Configuration);
        
    }
    public WebApplication Build()
    {
        var app = ApplicationBuilder.Build();
        ConfigureApp(app);
        return app;
    }

    protected virtual void ConfigureApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();             
    }
    
    protected virtual void ConfigureLogging(IServiceCollection services,ILoggingBuilder loggingBuilder)
    {
        services.AddLogging();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        
    }

    protected virtual void ConfigureConfiguration(ConfigurationManager configurationManager)
    {
        
    }
}