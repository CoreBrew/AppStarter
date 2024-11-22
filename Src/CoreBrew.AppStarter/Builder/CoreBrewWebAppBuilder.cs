using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

/// <summary>
///     Abstract base class setting up the fundamentals of the WebApp
/// </summary>
public abstract class CoreBrewWebAppBuilder : CoreBrewAppBuilder<WebApplicationBuilder>
{
    /// <summary>
    /// </summary>
    protected CoreBrewWebAppBuilder() : base(WebApplication.CreateBuilder(Environment.GetCommandLineArgs()))
    {
    }

    /// <inheritdoc />
    protected override void PreConfigure()
    {
        ApplicationBuilder.Services.AddControllers();
        ApplicationBuilder.Services.AddEndpointsApiExplorer();
        ApplicationBuilder.Services.AddSwaggerGen();
    }

    /// <inheritdoc />
    protected override void Configure()
    {
        ConfigureConfiguration(ApplicationBuilder.Configuration);
    }

    internal WebApplication Build()
    {
        var app = ApplicationBuilder.Build();
        SetupApp(app);
        return app;
    }

    /// <summary>
    ///     Configure app, override to customize app configuration
    /// </summary>
    /// <param name="app"></param>
    protected virtual void SetupApp(WebApplication app)
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
    

    /// <summary>
    /// Configure the configuration/options
    /// </summary>
    /// <param name="configurationManager"></param>
    protected virtual void ConfigureConfiguration(ConfigurationManager configurationManager)
    {
        
    }       
}