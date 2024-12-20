﻿using CoreBrew.AppStarter.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WPFSampleApplication;

public class CoreBrewHostApplicationBuilder : CoreBrew.AppStarter.Builder.CoreBrewHostApplicationBuilder
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddHostedService<TestHostedService>();
    }


}