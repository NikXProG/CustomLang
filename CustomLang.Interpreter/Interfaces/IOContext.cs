namespace CustomLang.Interpreter.interfaces;


/// <summary>
/// Defines the input/output context for interpreter operations
/// </summary>
public interface IIOContext
{
    /// <summary>
    /// Gets the output writer for displaying results and messages
    /// </summary>
    IOutputWriter OutputWriter { get; }
    
    /// <summary>
    /// Gets the input reader for receiving user input
    /// </summary>
    IInputReader InputReader { get; }
}