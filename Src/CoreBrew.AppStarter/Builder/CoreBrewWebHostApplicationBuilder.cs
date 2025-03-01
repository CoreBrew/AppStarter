using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

/// <summary>
///     Abstract base class setting up the fundamentals of the WebApp
/// </summary>
public abstract class WebCoreBrewHostApplicationBuilder : CoreBrewHostApplicationBuilderBase<WebApplication>
{
    /// <summary>
    /// </summary>
    protected WebCoreBrewHostApplicationBuilder() : base(WebApplication.CreateBuilder(Environment.GetCommandLineArgs()))
    {
    }

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        ApplicationBuilder.Services.AddControllers();
        ApplicationBuilder.Services.AddEndpointsApiExplorer();
        ApplicationBuilder.Services.AddSwaggerGen();        
    }

    /// <inheritdoc />
    protected override WebApplication BuildApp()
    {
        var appBuilder = (ApplicationBuilder as WebApplicationBuilder)!;
        var app  = appBuilder.Build();
        return app;
    }

    /// <inheritdoc />
    protected override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();        
    }
    
}