using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreBrew.AppStarter.Options;

/// <summary>
/// Load and store options 
/// </summary>
/// <param name="appSettingsFileName"></param>
/// <param name="serviceProvider"></param>
public class CoreBrewOptionsStore(string appSettingsFileName,IServiceProvider serviceProvider)
{
    private readonly SemaphoreSlim _lock = new(1,1);
    /// <summary>
    /// Loads options from the app settings file
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public async Task<TOptions?> LoadAsync<TOptions>() where TOptions : OptionsSection
    {
        await _lock.WaitAsync();

        try
        {
            var sectionName = serviceProvider.GetRequiredService<IOptions<TOptions>>().Value.SectionName();
            var fileContent = await File.ReadAllTextAsync(appSettingsFileName);
            using var doc = JsonDocument.Parse(fileContent);
            var root = doc.RootElement.Clone();
            return root.TryGetProperty(sectionName, out var element) ? element.Deserialize<TOptions>() : null;
        }
        finally
        {
            _lock.Release();
        }
    }
    
    /// <summary>
    /// Loads options from the app settings file
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public TOptions? Load<TOptions>() where TOptions : OptionsSection
    {
        _lock.Wait();
        try
        {
            var sectionName = serviceProvider.GetRequiredService<IOptions<TOptions>>().Value.SectionName();
            var fileContent = File.ReadAllText(appSettingsFileName);
            using var doc = JsonDocument.Parse(fileContent);
            var root = doc.RootElement.Clone();
            return root.TryGetProperty(sectionName, out var element) ? element.Deserialize<TOptions>() : null;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Saves back options to the app settings file
    /// </summary>
    /// <param name="optionsInstance"></param>
    /// <typeparam name="TOptions"></typeparam>
    public async Task SaveAsync<TOptions>(TOptions optionsInstance) where TOptions : OptionsSection
    {
        await _lock.WaitAsync();
        try
        {
            // Read the current JSON
            var sectionName = serviceProvider.GetRequiredService<IOptions<TOptions>>().Value.SectionName();
            var fileContent = await File.ReadAllTextAsync(appSettingsFileName);

            var updatedJson = GetUpdatedJsonText(optionsInstance, fileContent, sectionName);
            await File.WriteAllTextAsync(appSettingsFileName, updatedJson);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Saves back options to the app settings file 
    /// </summary>
    /// <param name="optionsInstance"></param>
    /// <typeparam name="TOptions"></typeparam>
    public void Save<TOptions>(TOptions optionsInstance) where TOptions : OptionsSection
    {
        _lock.Wait();
        try
        {
            // Read the current JSON
            var sectionName = serviceProvider.GetRequiredService<IOptions<TOptions>>().Value.SectionName();
            var fileContent = File.ReadAllText(appSettingsFileName);

            var updatedJson = GetUpdatedJsonText(optionsInstance, fileContent, sectionName);
            File.WriteAllText(appSettingsFileName, updatedJson);            
        }
        finally
        {
            _lock.Release();
        }
    }
    
    private static string GetUpdatedJsonText<TOptions>(TOptions optionsInstance, string fileContent, string sectionName)
        where TOptions : OptionsSection
    {
        using var doc = JsonDocument.Parse(fileContent);
        // Add the section
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(fileContent) ?? new();
        dictionary[sectionName] = optionsInstance;
        // Write back to file
        var updatedJson = JsonSerializer.Serialize(dictionary, new JsonSerializerOptions { WriteIndented = true });
        return updatedJson;
    }
}