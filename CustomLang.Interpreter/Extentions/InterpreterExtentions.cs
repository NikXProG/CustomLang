using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.Visitors;
using CustomLang.Interpreter.Visitors.LangObj;
using String = CustomLang.Interpreter.Visitors.LangObj.String;

namespace CustomLang.Interpreter.Extentions;

public static class InterpreterExtentions
{
    public static LangObject ConvertTo<T>(this TokenTypeExpressionNode node)
    {
        
        switch (node.LiteralType)
        {
            case SystemType.String:
                break;
        }
        return new String("sdsds");
    }
}