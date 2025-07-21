namespace CustomLang.Interpreter.Exceptions;

/// <summary>
/// Exception indicating debugger continue command
/// </summary>
public class DebugContinueException : Exception
{
    public DebugContinueException() : base("Debugger continue") { }
}
