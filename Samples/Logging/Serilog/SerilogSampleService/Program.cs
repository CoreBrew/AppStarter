using CoreBrew.AppStarter.Builder;
using Microsoft.Extensions.Hosting;
using SerilogSampleService;

CoreBrewApplicationHostFactory.Build(new AppBuilder()).Run();