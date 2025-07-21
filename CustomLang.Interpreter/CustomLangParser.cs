using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.interfaces;
using Sprache;
using CustomLang.Interpreter.AstNodes;
using CustomLang.Interpreter.config;
using CustomLang.Interpreter.Extentions;

namespace CustomLang.Interpreter;

public class CustomLangParser : ICustomLangParser
{
    #region Fields
    
    private readonly CustomLangGrammar _grammar;
    
    #endregion
    
    #region Constructors
    
    public CustomLangParser(CustomLangGrammar grammar)
    {
        _grammar = grammar;
    }
    
    #endregion
    
   
    #region Methods
    
    public StatementNode Parse(string text) =>
        _grammar.AstNodeExpression.ParseEx(text);
    
    public ExpressionNode ParseExpression(string text) =>
        _grammar.Expr.Parse(text);
    
    #endregion
    
}
