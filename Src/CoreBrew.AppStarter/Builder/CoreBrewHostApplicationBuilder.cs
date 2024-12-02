using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

/// <inheritdoc />
public abstract class CoreBrewHostApplicationBuilder : CoreBrewHostApplicationBuilderBase<IHost>
{
    /// <inheritdoc />
    protected CoreBrewHostApplicationBuilder() : base(new HostApplicationBuilder(Environment.GetCommandLineArgs()))
    {
    }

    /// <inheritdoc />
    protected override IHost BuildApp()
    {
        var app = (ApplicationBuilder as HostApplicationBuilder)!.Build();
        return app;
    }
}