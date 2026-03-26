using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBrew.AppStarter.Options;

/// <summary>
/// Simple options binder helper
/// </summary>
public class CoreBrewOptionsBinder
{
    private readonly IHostApplicationBuilder _applicationBuilder;
    private readonly string _settingsFilePath;

    /// <summary>
    /// Create a new instances of the <see cref="CoreBrewOptionsBinder"/>
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="settingsFilePath"></param>
    /// <exception cref="Exception"></exception>
    public CoreBrewOptionsBinder(IHostApplicationBuilder applicationBuilder, string settingsFilePath)
    {
        _applicationBuilder = applicationBuilder;
        _settingsFilePath = settingsFilePath;
    }

    /// <summary>
    /// Add options section and bind ti
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public CoreBrewOptionsBinder AddOptions<TOptions>() where TOptions : OptionsSection
    {
        var servicesCollection = _applicationBuilder.Services;
        var configurationManager = _applicationBuilder.Configuration;
        var optionsInstance = Activator.CreateInstance<TOptions>();
        var sectionName = optionsInstance.SectionName();

        servicesCollection
            .AddOptions<TOptions>()
            .Bind(configurationManager.GetSection(sectionName));

        // Ensure section exists in appSettings
        EnsureSectionInSettingsFile(sectionName, optionsInstance);

        return this;
    }

    /// <summary>
    /// Ensures that a given section exists in the appsettings.json file.
    /// If it does not exist, the default values from the options instance are written.
    /// </summary>
    private void EnsureSectionInSettingsFile<TOptions>(string sectionName, TOptions optionsInstance)
        where TOptions : class
    {
        // Read the current JSON
        var jsonContent = File.Exists(_settingsFilePath)
            ? File.ReadAllText(_settingsFilePath)
            : "{}";

        using var doc = JsonDocument.Parse(jsonContent);
        var root = doc.RootElement.Clone();

        if (!root.TryGetProperty(sectionName, out _))
        {
            // Add the section
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent) ?? new();
            dictionary[sectionName] = optionsInstance;

            // Write back to file
            var updatedJson = JsonSerializer.Serialize(dictionary, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsFilePath, updatedJson);
        }
    }
}