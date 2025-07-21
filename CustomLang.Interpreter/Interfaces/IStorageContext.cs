using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.Visitors.LangObj;
using CustomLang.Storage;

namespace CustomLang.Interpreter.interfaces;

/// <summary>
/// Defines the storage context for variables and functions during execution
/// </summary>
public interface IStorageContext
{ 
    /// <summary>
    /// Gets the storage for runtime variables
    /// </summary>
    IInterpreterStorage<LangObject> VariablesStorage { get; }
    
    /// <summary>
    /// Gets the storage for function declarations
    /// </summary>
    IInterpreterStorage<FunctionStatementNode> FunctionsStorage { get; }
}