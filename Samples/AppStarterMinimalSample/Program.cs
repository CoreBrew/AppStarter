using AppStarterMinimalSample;
using CoreBrew.AppStarter.Builder;

var app = CoreBrewApplicationCreator.CreateWebApplication(new AppBuilder(args));
app.Run();