using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

/// <inheritdoc />
public abstract class CoreBrewHostApplicationBuilder : CoreBrewHostApplicationBuilderBase
{
    /// <inheritdoc />
    protected CoreBrewHostApplicationBuilder() : base(new Microsoft.Extensions.Hosting.HostApplicationBuilder(Environment.GetCommandLineArgs()))
    {
    }

    /// <inheritdoc />
    protected override IHost BuildApp()
    {
        var app = (ApplicationBuilder as Microsoft.Extensions.Hosting.HostApplicationBuilder)!.Build();
        return app;
    }
}