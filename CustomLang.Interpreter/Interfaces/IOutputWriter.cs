namespace CustomLang.Interpreter.interfaces;

public interface IOutputWriter
{
    void WriteLine(string? message);
    void WriteLine();
    void Write(string? message);
    
}
