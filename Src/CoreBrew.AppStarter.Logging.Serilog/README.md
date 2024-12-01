
# CoreBrew.AppStarter.Logging.Serilog

**CoreBrew.AppStarter.Logging.Serilog** is a .NET library that integrates [Serilog](https://serilog.net) logging capabilities into applications built with the CoreBrew AppStarter framework. It provides a streamlined way to configure and extend logging with Serilog.

## Features

- Seamless integration with the CoreBrew AppStarter framework.
- Configurable logging pipeline using Serilog's powerful capabilities.
- Extensible design for advanced logging scenarios.

## Requirements

- .NET 8.0 or higher
- CoreBrew AppStarter framework

## Installation

To add this library to your project, include it as a dependency in your solution. If released as a NuGet package, you can install it using the following command:

```bash
dotnet add package CoreBrew.AppStarter.Logging.Serilog
```

## Usage

### Setting Up Logging with Serilog

1. Create a class inheriting from `CoreBrewHostApplicationBuilder` 
2. Extend your application using the `SerilogApplicationExtension` class, in the AddHostAppExtension method
3. Run it.

#### AppBuilder.cs
```csharp
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
```
#### Program.cs
```csharp
using CoreBrew.AppStarter.Builder;
using SerilogSampleService;

CoreBrewApplicationHostFactory.Build(new AppBuilder()).Run();
```


## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

### Steps to Contribute

1. Clone the repository.
2. Create a new branch for your feature or bugfix.
3. Commit your changes and push them to your branch.
4. Open a pull request.

## License

This library is provided under the [MIT License](LICENSE.md).

## Acknowledgments

- [Serilog](https://serilog.net)
- CoreBrew AppStarter framework

