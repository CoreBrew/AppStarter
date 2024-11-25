using AppStarterMinimalSample.AddInTest.AddInFromAddIn;
using CoreBrew.AppStarter.Builder;
using CoreBrew.AppStarter.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppStarterMinimalSample.AddInTest;

public class MyAddIn(IHostApplicationBuilder applicationBuilder) : CoreBrewHostApplicationAddIn(applicationBuilder)
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.TryAddSingleton<MyServiceClass>();
    }

    protected override void ConfigureConfiguration(IConfigurationManager configurationManager, CoreBrewOptionsBinder optionsBinder)
    {
        base.ConfigureConfiguration(configurationManager, optionsBinder);
        optionsBinder.AddOptions<MyAddInOption>();
    }

    protected override void AddAddIns(AddInCollection addIns, IHostApplicationBuilder builder)
    {
        base.AddAddIns(addIns, builder);
        addIns.Add(new MyAddInAddedAddIn(builder));
    }
}