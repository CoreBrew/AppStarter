using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

/// <summary>
///     Abstract base class setting up the fundamentals of the WebApp
/// </summary>
public abstract class CoreBrewBlazorHostApplicationBuilder : CoreBrewHostApplicationBuilderBase<WebApplication>
{
    /// <summary>
    /// </summary>
    protected CoreBrewBlazorHostApplicationBuilder() : base(WebApplication.CreateBuilder(Environment.GetCommandLineArgs()))
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
        var app = (ApplicationBuilder as WebApplicationBuilder)!.Build();
        return app;
    }

    /// <inheritdoc />
    protected override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();
    }
    
}