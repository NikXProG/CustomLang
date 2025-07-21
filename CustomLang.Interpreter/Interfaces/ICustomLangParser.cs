using CustomLang.Interpreter.AstNodes.Types;

namespace CustomLang.Interpreter.interfaces;

/// <summary>
/// Defines the core parser interface for CustomLang language processing
/// </summary>
public interface ICustomLangParser
{
    /// <summary>
    /// Parses complete source code into an abstract syntax tree (AST)
    /// </summary>
    /// <param name="code">The source code to parse</param>
    /// <returns>The root <see cref="StatementNode"/> of the generated AST</returns>
    /// <exception cref="ArgumentNullException">Thrown when code is null or empty</exception>
    /// <exception>Thrown when syntax errors are encountered</exception>
    StatementNode Parse(string code);

    /// <summary>
    /// Parses a single expression into an expression tree node
    /// </summary>
    /// <param name="text">The expression text to parse</param>
    /// <returns>The root <see cref="ExpressionNode"/> of the parsed expression</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null or empty</exception>
    /// <exception>Thrown when expression contains syntax errors</exception>
    ExpressionNode ParseExpression(string text);
}

