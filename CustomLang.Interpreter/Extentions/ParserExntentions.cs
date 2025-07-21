using System.Numerics;
using CustomLang.Interpreter.Exceptions;
using CustomLang.Interpreter.interfaces;
using Sprache;
using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.Visitors.LangObj;
using Double = CustomLang.Interpreter.Visitors.LangObj.Double;

namespace CustomLang.Interpreter.Extentions;

public static class ParserExtensions
{
    
    
    public static T ParseEx<T>(this Parser<T> parser, string input)
    {
        
        var result = parser.TryParse(input);
        
        if (result.WasSuccessful)
        {
            return result.Value;
        }


        throw new CustomLangException(result.ToString(),
            result.Remainder.Line,
            result.Remainder.Column,
            (input ?? string.Empty).Split(new[] { "\r\n", "\n" },
                StringSplitOptions.None));
    }



    public static Parser<T> Token<T>(this Parser<T> parser, ICommentParser provider)
    {
        var trailingCommentParser =
            provider?.CommentParser?.AnyComment?.Token() ??
            Parse.WhiteSpace.Many().Text();
        
        return
            from value in parser.Commented(provider).Token()
            from comment in trailingCommentParser.Many()
            select value.Value;
    }
    
    public static Parser<ICommented<T>> Commented<T>(this Parser<T> parser, ICommentParser provider)
    {
        return parser.Commented(provider?.CommentParser);
    }
    
    
    
}

