using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Builder;

/// <inheritdoc />
public abstract class CoreBrewHostApplicationBuilder : CoreBrewHostApplicationBuilderBase<HostApplicationBuilder>
{
    /// <inheritdoc />
    protected CoreBrewHostApplicationBuilder() : base(new HostApplicationBuilder(Environment.GetCommandLineArgs()))
    {
        
    }

    /// <inheritdoc />
    public override IHost Build()
    {
        var app = ApplicationBuilder.Build();
        return app;        
    }
}