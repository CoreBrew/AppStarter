using System.Runtime.InteropServices;
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
        var appBuilder = (ApplicationBuilder as HostApplicationBuilder)!;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            appBuilder.Services.AddWindowsService();            
        }
        var app = appBuilder.Build();
        return app;
    }
}