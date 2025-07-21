namespace CustomLang.Interpreter.Config;

/// <summary>
/// Represents the types of operations supported in the custom language
/// </summary>
public enum TypeOperation
{
    Add, // Addition operation
    Subtract, // Subtraction operation
    Multiply, //Multiplication operation
    Divide, // Division operation
    Modulus, // Modulus operation
    Power, // Power operation
    Assign // Assignment operation
}

/// <summary>
/// Provides a fluent interface for configuring language keywords and operations
/// </summary>
public class KeywordConfiguration
{
    
    #region Fields
    
    private readonly CustomLangGrammarConfig _grammarConfig;

    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the KeywordConfiguration class
    /// </summary>
    /// <param name="grammarConfig">The grammar configuration to modify</param>
    public KeywordConfiguration(CustomLangGrammarConfig grammarConfig)
    {
        _grammarConfig = grammarConfig;
    }
    
    #endregion

    #region Syntax Methods
    
    /// <summary>
    /// Sets the keyword for float type
    /// </summary>
    /// <param name="value">The float type keyword</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetFloat(string value)
    {
        _grammarConfig.Float = value;
        return this;
    }

    /// <summary>
    /// Sets the keyword for void type
    /// </summary>
    /// <param name="value">The void type keyword</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetVoid(string value)
    {
        _grammarConfig.Void = value; 
        return this;
    }

    /// <summary>
    /// Sets the keyword for integer type
    /// </summary>
    /// <param name="value">The integer type keyword</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetInt(string value)
    {
        _grammarConfig.Int = value; 
        return this;
    }

    /// <summary>
    /// Sets the keyword for string type
    /// </summary>
    /// <param name="value">The string type keyword</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetString(string value)
    {
        _grammarConfig.String = value;
        return this;
    }

    /// <summary>
    /// Sets the return statement keyword
    /// </summary>
    /// <param name="value">The return keyword</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetReturn(string value)
    {
        _grammarConfig.Return = value; 
        return this;
    }

    /// <summary>
    /// Sets the output operation keyword
    /// </summary>
    /// <param name="value">The output keyword</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetOutput(string value)
    {
        _grammarConfig.Output = value;
        return this;
    }
    
    /// <summary>
    /// Sets the input operation keyword
    /// </summary>
    /// <param name="value">The input keyword</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetInput(string value)
    {
        _grammarConfig.Input = value;
        return this;
    }

    /// <summary>
    /// Sets the function declaration keyword
    /// </summary>
    /// <param name="value">The function keyword</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetFunctionSign(string value)
    {
        _grammarConfig.Function = value; 
        return this;
    }
    
    /// <summary>
    /// Sets the include directive keyword
    /// </summary>
    /// <param name="value">The include keyword</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetInclude(string value)
    {
        _grammarConfig.Include = value; 
        return this;
    }
    
    #endregion
    
    #region Operation Methods

    /// <summary>
    /// Sets the symbol for a specific operation type
    /// </summary>
    /// <param name="operation">The operation type to configure</param>
    /// <param name="value">The symbol representing the operation</param>
    /// <returns>This instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when unsupported operation type is provided</exception>
    public KeywordConfiguration SetOperation(TypeOperation operation, string value)
    {
        switch (operation)
        {
            case TypeOperation.Add:
                _grammarConfig.Plus = value;
                break;
            case TypeOperation.Subtract:
                _grammarConfig.Minus = value;
                break;
            case TypeOperation.Multiply:
                _grammarConfig.Multiply = value;
                break;
            case TypeOperation.Divide:
                _grammarConfig.Divide = value;
                break;
            case TypeOperation.Modulus:
                _grammarConfig.Modulo = value;
                break;
            case TypeOperation.Power:
                _grammarConfig.Power = value;
                break;
            case TypeOperation.Assign:
                _grammarConfig.Assign = value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
        }

        return this;
    }
    #endregion

    #region Syntax Style Methods

    /// <summary>
    /// Sets the assignment style for the language
    /// </summary>
    /// <param name="style">The assignment style to use</param>
    /// <returns>This instance for method chaining</returns>
    public KeywordConfiguration SetAssignmentStyle(AssignmentStyle style)
    {
        _grammarConfig.AssignmentStyle = style; 
        return this;
    }
    
    #endregion
    
}