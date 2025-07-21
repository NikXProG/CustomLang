using CustomLang.Config.Parser;
using CustomLang.Interpreter.Config;
using CustomLang.Interpreter.interfaces;

namespace CustomLang.Interpreter.config;
/// <summary>
/// Interpreter configuration settings for number bases and debug mode
/// </summary>
public class InterpreterConfig
{
    #region Constants
    /// <summary>
    /// INI file section name for number base settings
    /// </summary>
    private const string NumberBaseSettings = "NumberBaseSettings";
    #endregion

    #region Properties
    /// <summary>
    /// Numeric base used for assignment operations
    /// </summary>
    /// <remarks>
    /// Default: 10
    /// </remarks>
    [IniProperty(NumberBaseSettings, "AssignmentBase")]
    public int AssignmentBase { get; set; } = 10;

    /// <summary>
    /// Numeric base used for input operations
    /// </summary>
    /// <remarks>
    /// Default: 10 
    /// </remarks>
    [IniProperty(NumberBaseSettings, "InputBase")]
    public int InputBase { get; set; } = 10;

    /// <summary>
    /// Numeric base used for output operations
    /// </summary>
    /// <remarks>
    /// Default: 10 
    /// </remarks>
    [IniProperty(NumberBaseSettings, "OutputBase")]
    public int OutputBase { get; set; } = 10;

    /// <summary>
    /// Debug mode flag
    /// </summary>
    /// <remarks>
    /// When enabled, outputs additional diagnostic information
    /// Default: false (disabled)
    /// </remarks>
    [IniProperty(NumberBaseSettings, "DebugMode")]
    public bool DebugMode { get; set; }
    #endregion
}