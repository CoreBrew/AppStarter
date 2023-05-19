using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace CoreBrew.AppStarter.Builder;

public class CoreBrewWebAppBuilder : IApplicationBuilder
{
    private IApplicationBuilder _applicationBuilderImplementation;
    public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        return _applicationBuilderImplementation.Use(middleware);
    }

    public IApplicationBuilder New()
    {
        return _applicationBuilderImplementation.New();
    }

    public RequestDelegate Build()
    {
        return _applicationBuilderImplementation.Build();
    }

    public IServiceProvider ApplicationServices
    {
        get => _applicationBuilderImplementation.ApplicationServices;
        set => _applicationBuilderImplementation.ApplicationServices = value;
    }

    public IFeatureCollection ServerFeatures => _applicationBuilderImplementation.ServerFeatures;

    public IDictionary<string, object?> Properties => _applicationBuilderImplementation.Properties;
}