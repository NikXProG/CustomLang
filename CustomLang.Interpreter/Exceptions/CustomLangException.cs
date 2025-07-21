using Sprache;

namespace CustomLang.Interpreter.Exceptions;

public class CustomLangException: ParseException
{
    public string[] CustomLang { get; set; }
    public int LineNumber { get; set; }
    public int ColumnPosition { get; set; }
    public CustomLangException(
        string message,
        int lineNumber,
        int columnPosition,
        string[] customLang)
        : base(message)
    {
        LineNumber = lineNumber;
        CustomLang = customLang;
        ColumnPosition = columnPosition;
    }
}