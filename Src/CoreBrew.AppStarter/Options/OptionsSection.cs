namespace CoreBrew.AppStarter.Options;

/// <summary>
/// A base class, that has a section name to load from options
/// </summary>
public class OptionsSection
{
    /// <summary>
    /// The section name to load from the option file
    /// </summary>
    public virtual string SectionName() => GetType().Name;
}