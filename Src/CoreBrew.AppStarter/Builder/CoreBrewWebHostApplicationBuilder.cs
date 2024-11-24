using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

/// <summary>
///     Abstract base class setting up the fundamentals of the WebApp
/// </summary>
public abstract class CoreBrewWebHostApplicationBuilder : CoreBrewHostApplicationBuilderBase<WebApplicationBuilder>
{
    /// <summary>
    /// </summary>
    protected CoreBrewWebHostApplicationBuilder() : base(WebApplication.CreateBuilder(Environment.GetCommandLineArgs()))
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
    protected override IHost BuildApp()
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
}