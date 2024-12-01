using CoreBrew.AppStarter.Builder;
using CoreBrew.AppStarter.Logging.Serilog;

namespace SerilogSampleService;

public class AppBuilder : CoreBrewHostApplicationBuilder
{
    protected override void AddHostAppExtensions(HostApplicationExtensionRegistry hostApplicationExtensionRegistry)
    {
        base.AddHostAppExtensions(hostApplicationExtensionRegistry);
        hostApplicationExtensionRegistry.Register<SerilogApplicationExtension>();
    }
}