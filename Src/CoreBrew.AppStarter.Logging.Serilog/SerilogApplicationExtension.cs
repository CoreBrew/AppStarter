using CoreBrew.AppStarter.Builder;
using Microsoft.Extensions.Configuration;
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

        var serilogConfiguration = ApplicationBuilder.Configuration.GetSection("Serilog");
        var logConfiguration = serilogConfiguration.Exists()
            ? new LoggerConfiguration().ReadFrom.Configuration(ApplicationBuilder.Configuration)
            : SetCoreBrewDefaultLogConfiguration();

        loggingBuilder.AddSerilog(logConfiguration.CreateLogger());
    }

    private static LoggerConfiguration SetCoreBrewDefaultLogConfiguration()
    {
        return new LoggerConfiguration()
            .WriteTo.File(path: "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Properties}{NewLine}{Exception}",
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 20_000_000,
                retainedFileCountLimit: 10)
            .Enrich.FromLogContext()
            .Enrich.WithThreadName()
            .Enrich.WithThreadId();
    }
}