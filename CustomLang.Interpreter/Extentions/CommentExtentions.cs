
using System.Collections;
using CustomLang.Interpreter.AstNodes;
using CustomLang.Interpreter.AstNodes.Types;

namespace CustomLang.Interpreter.Extentions;

public static class CommentExtentions
{
    public static T WithLeadingComments<T>(this T syntax, IEnumerable<Comment> comments)
        where T : AstNode
    {
        if (comments != null && syntax != null)
        {
            syntax.LeadingComments.AddRange(comments);
        }

        return syntax;
    }

    // public static T WithLeadingComments<T>(this T syntax, IEnumerable<string> comments)
    //     where T : AstNode
    // {
    //     if (comments != null && syntax != null)
    //     {
    //         syntax.LeadingComments.AddRange(comments);
    //     }
    //
    //     return syntax;
    // }

    public static T WithLeadingComment<T>(this T syntax, Comment comment)
        where T : AstNode
    {
        if (comment != null && syntax != null)
        {
            syntax.LeadingComments.Add(comment);
        }

        return syntax;
    }
    // public static T WithLeadingComment<T>(this T syntax, string comment)
    //     where T : AstNode
    // {
    //     if (comment != null && syntax != null)
    //     {
    //         syntax.LeadingComments.Add(comment);
    //     }
    //
    //     return syntax;
    // }

    public static T WithTrailingComments<T>(this T syntax, IEnumerable<Comment> comments)
        where T : AstNode
    {
        if (comments != null && syntax != null)
        {
            syntax.TrailingComments.AddRange(comments);
        }

        return syntax;
    }

    // public static T WithTrailingComments<T>(this T syntax, IEnumerable<string> comments)
    //     where T : AstNode
    // {
    //     if (comments != null && syntax != null)
    //     {
    //         syntax.TrailingComments.AddRange(comments);
    //     }
    //
    //     return syntax;
    // }

    public static T WithTrailingComment<T>(this T syntax, Comment comment)
        where T : AstNode
    {
        if (comment != null && syntax != null)
        {
            syntax.TrailingComments.Add(comment);
        }

        return syntax;
    }

    // public static T WithTrailingComment<T>(this T syntax, string comment)
    //     where T : AstNode
    // {
    //     if (comment != null && syntax != null)
    //     {
    //         syntax.TrailingComments.Add(comment);
    //     }
    //
    //     return syntax;
    // }

    public static BlockStatementNode WithInnerComments(this BlockStatementNode syntax, IEnumerable<string> comments)
    {
        if (comments != null && syntax != null)
        {
            syntax.InnerComments.AddRange(comments);
        }

        return syntax;
    }

    public static BlockStatementNode WithInnerComment(this BlockStatementNode syntax, params string[] comments) =>
        syntax.WithInnerComments(comments);

    // public static ClassDeclarationSyntax WithInnerComments(this ClassDeclarationSyntax syntax, IEnumerable<string> comments)
    // {
    //     if (comments != null && syntax != null)
    //     {
    //         syntax.InnerComments.AddRange(comments);
    //     }
    //
    //     return syntax;
    // }
    //
    // public static ClassDeclarationSyntax WithInnerComment(this ClassDeclarationSyntax syntax, params string[] comments) =>
    //     syntax.WithInnerComments(comments);


    private static List<T> Concat<T>(List<T> first, List<T> second) =>
        first.EmptyIfNull().Concat(second.EmptyIfNull()).ToList();


}
