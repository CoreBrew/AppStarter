using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

/// <inheritdoc />
public abstract class CoreBrewHostApplicationBuilder : CoreBrewHostApplicationBuilderBase<IHost>
{
    /// <inheritdoc />
    protected CoreBrewHostApplicationBuilder() : base(new HostApplicationBuilder(System.Environment.GetCommandLineArgs()))
    {
    }

    /// <inheritdoc />
    protected override IHost BuildApp()
    {
        var appBuilder = (ApplicationBuilder as HostApplicationBuilder)!;
        var app = appBuilder.Build();
        return app;
    }
}