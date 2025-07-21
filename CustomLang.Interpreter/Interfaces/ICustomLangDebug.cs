namespace CustomLang.Interpreter.interfaces;

/// <summary>
/// Defines the debugger interface for interactive debugging sessions
/// </summary>
public interface ICustomLangDebug
{
    /// <summary>
    /// Enters interactive debugging mode
    /// </summary>
    /// <remarks>
    /// While in debug mode:
    /// 1. Execution is paused at breakpoints
    /// 2. Debug commands can inspect/modify state
    /// 3. Execution can be continued or terminated
    /// </remarks>
    void EnterInteractiveMode();
}