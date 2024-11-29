using CoreBrew.AppStarter.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CoreBrew.AppStarter.Logging.Serilog;

public class SerilogApplicationExtension : CoreBrewHostApplicationExtension
{
    protected override void ConfigureLogging(IServiceCollection services, ILoggingBuilder loggingBuilder)
    {
        base.ConfigureLogging(services, loggingBuilder);
        loggingBuilder.ClearProviders();

        var logConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithThreadName()
            .Enrich.WithThreadId()
            
            //Allow configuration to override defaults
            .ReadFrom.Configuration(ApplicationBuilder.Configuration);
        loggingBuilder.AddSerilog(logConfiguration.CreateLogger());
    }
}