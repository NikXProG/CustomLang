namespace CustomLang.Interpreter.Exceptions;


/// <summary>
/// Exception indicating debugger termination command
/// </summary>
public class DebugTerminateException : Exception
{
    public DebugTerminateException() : base("Debugger terminate") { }
}