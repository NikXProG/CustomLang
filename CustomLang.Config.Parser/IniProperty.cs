namespace CustomLang.Config.Parser;


/// <summary>
/// Attribute that marks a property for automatic loading from a .ini configuration file.
/// </summary>
/// <remarks>
/// This attribute specifies how a property should be mapped to a key-value pair in an INI file.
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
public class IniPropertyAttribute : Attribute
{
    /// <summary>
    /// Gets the section name in the INI file where the key is located.
    /// </summary>
    /// <value>
    /// The section name enclosed in square brackets in the INI file.
    /// </value>
    /// <example>
    /// For an INI file structure:
    /// <code>
    /// [Network]
    /// Timeout = 30
    /// </code>
    /// The section would be "Network".
    /// </example>
    public string Section { get; }
    
    /// <summary>
    /// Gets the key name in the INI file that contains the value for this property.
    /// </summary>
    /// <value>
    /// The key name that appears before the equals sign in the INI file.
    /// </value>
    /// <example>
    /// For an INI file structure:
    /// <code>
    /// [Network]
    /// Timeout = 30
    /// </code>
    /// The key would be "Timeout".
    /// </example>
    public string Key { get; }
    
    /// <summary>
    /// Gets a value indicating whether this property is optional when reading the configuration.
    /// </summary>
    /// <value>
    /// <c>true</c> if the property is optional; <c>false</c> if a missing key should cause an error.
    /// </value>
    /// <remarks>
    /// When set to false, the configuration loader will throw an exception if the specified section/key
    /// is not found in the INI file.
    /// </remarks>
    public bool Optional { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IniPropertyAttribute"/> class.
    /// </summary>
    /// <param name="section">The section name in the INI file.</param>
    /// <param name="key">The key name in the specified section.</param>
    /// <param name="optional">Whether the property is optional (default: true).</param>
    public IniPropertyAttribute(string section, string key, bool optional = true)
    {
        Section = section;
        Key = key;
        Optional = optional;
    }
}