namespace CustomLang.Interpreter.config;

/// <summary>
/// Builder class for creating and configuring <see cref="InterpreterConfig"/> instances.
/// Implements fluent interface pattern for method chaining.
/// </summary>
public class InterpreterConfigBuilder
{
    #region Fields
    private int _outputBase = 10;
    private int _inputBase = 10;
    private int _assignmentBase = 2;
    private bool _debugMode;
    #endregion

    #region Methods
    
    /// <summary>
    /// Sets the numeric base for output operations
    /// </summary>
    /// <param name="outputBase">The base to use for output (default: 10)</param>
    /// <returns>This builder instance for method chaining</returns>
    public InterpreterConfigBuilder SetOutputBase(int outputBase)
    {
        
        _outputBase = outputBase;
        return this;
    }
    
    /// <summary>
    /// Sets the numeric base for input operations
    /// </summary>
    /// <param name="inputBase">The base to use for input (default: 10)</param>
    /// <returns>This builder instance for method chaining</returns>
    public InterpreterConfigBuilder SetInputBase(int inputBase)
    {
        _inputBase = inputBase;
        return this;
    }
    
    /// <summary>
    /// Sets the numeric base for assignment operations
    /// </summary>
    /// <param name="assignmentBase">The base to use for assignments (default: 2)</param>
    /// <returns>This builder instance for method chaining</returns>
    public InterpreterConfigBuilder SetAssignmentBase(int assignmentBase)
    {
        _assignmentBase = assignmentBase;
        return this;
    }
    
    /// <summary>
    /// Enables or disables debug mode
    /// </summary>
    /// <param name="debugMode">True to enable debug mode, false to disable</param>
    /// <returns>This builder instance for method chaining</returns>
    public InterpreterConfigBuilder SetDebugMode(bool debugMode)
    {
        _debugMode = debugMode;
        return this;
    }
    
    /// <summary>
    /// Constructs a new <see cref="InterpreterConfig"/> instance with the configured settings
    /// </summary>
    /// <returns>A new configured instance of <see cref="InterpreterConfig"/></returns>
    public InterpreterConfig Build()
    {
        return new InterpreterConfig
        {
            OutputBase = _outputBase,
            InputBase = _inputBase,
            AssignmentBase = _assignmentBase,
            DebugMode = _debugMode
        };
    }
    
    #endregion
}