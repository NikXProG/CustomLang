using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using ApexParser.Toolbox;
using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.Config;
using CustomLang.Interpreter.interfaces;
using Sprache;
using CustomLang.Interpreter.AstNodes;
using CustomLang.Interpreter.Extentions;

namespace CustomLang.Interpreter;
public class CustomLangGrammar : ICommentParser
{
    
    #region Fields

    private readonly CustomLangGrammarConfig _grammarConfig;

    #endregion
    
    #region Constructors
    
    public CustomLangGrammar(CustomLangGrammarConfig config)
    {
        _grammarConfig = config ?? throw new ArgumentNullException(nameof(config));
        CommentParser = new AdaptCommentParser(
            config.SingleLineComment,
            config.MultiLineOpenComment,
            config.MultiLineCloseComment, 
            config.NewLineComment);
    }
    
    #endregion
    
    #region Grammar Language
    
    #region Comment Parsing
    
    /// <summary>
    /// Gets the comment parser instance
    /// </summary>
    public IComment CommentParser { get; }
    
    /// <summary>
    /// Creates a comment from text
    /// </summary>
    /// <param name="text">Comment text</param>
    /// <returns>Comment object</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null or whitespace</exception>
    public Comment CreateComment(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));
            
        bool isSingle = text.StartsWith(_grammarConfig.SingleLineComment, StringComparison.Ordinal);
        
        string commentText = isSingle 
            ? text.Substring(_grammarConfig.SingleLineComment.Length).Trim()
            : text.Substring(
                    _grammarConfig.MultiLineOpenComment.Length, 
                    text.Length - _grammarConfig.MultiLineOpenComment.Length - _grammarConfig.MultiLineCloseComment.Length)
                .Trim();
        
        return new Comment
        {
            Text = commentText,
            IsSingle = isSingle,
        };
    }
    
    /// <summary>
    /// Creates multiple comments from a collection of strings
    /// </summary>
    /// <param name="comments">Collection of comment strings</param>
    /// <returns>Enumerable of Comment objects</returns>
    public IEnumerable<Comment> CreateComments(IEnumerable<string> comments)
    {
        return comments?.Select(CreateComment) ?? Enumerable.Empty<Comment>();
    }
    
    #endregion

    #region Base Parsers
    
    private const string NumberSystemSymbols36 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
    
    /// <summary>
    /// Parses raw identifiers (excluding reserved words)
    /// </summary>
    protected Parser<string> RawIdentifier =>
        from id in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit.Or(Parse.Char('_')))
        where !_grammarConfig.ReservedWords.Contains(id)
        select id;
        
    /// <summary>
    /// Parses tokenized identifiers
    /// </summary>
    protected Parser<string> Identifier => RawIdentifier.Token().Named("Identifier");
    
    /// <summary>
    /// Parses integer expressions (including decimals)
    /// </summary>
    protected Parser<TokenTypeExpressionNode> IntegerTypeExpression =>
        from integerPart in Parse.Chars(NumberSystemSymbols36).AtLeastOnce().Text()
        from next in Parse.Not(Parse.Char('.'))
        select new TokenTypeExpressionNode(integerPart, SystemType.Integer);
    
    
    /// <summary>
    /// Parses numeric expressions (including decimals)
    /// </summary>
    protected Parser<TokenTypeExpressionNode> NumericTypeExpression =>
        from integerPart in Parse.Chars(NumberSystemSymbols36).AtLeastOnce().Text()
        from maybeFloat in Parse.Char('.')
            .Then(delimiter => 
                Parse.Chars(NumberSystemSymbols36).AtLeastOnce().Text()
                    .Select(fractionPart => 
                        new TokenTypeExpressionNode($"{integerPart}.{fractionPart}", SystemType.Float)))
            .Optional()
        select maybeFloat.GetOrDefault() ?? new TokenTypeExpressionNode(integerPart, SystemType.Integer);
    /// <summary>
    /// Parses string literals
    /// </summary>
    protected Parser<TokenTypeExpressionNode> StringTypeExpression =>
        from token in StringLiteral
        select new TokenTypeExpressionNode(
            token.Trim('\''),
            SystemType.String);
        
    /// <summary>
    /// Parses any literal expression (numeric or string)
    /// </summary>
    protected Parser<TokenTypeExpressionNode> LiteralExpression =>
        NumericTypeExpression.XOr(StringTypeExpression);

    /// <summary>
    /// Parses string literals with escape sequences
    /// </summary>
    protected Parser<string> StringLiteral =>
        from leading in Parse.WhiteSpace.Many()
        from openQuote in Parse.Char('\'')
        from fragments in Parse.Char('\\').Then(_ => Parse.AnyChar.Select(c => $"\\{c}"))
            .Or(Parse.CharExcept("\\'").Many().Text()).Many()
        from closeQuote in Parse.Char('\'')
        from trailing in Parse.WhiteSpace.Many()
        select $"'{string.Join(string.Empty, fragments)}'";
    
    #endregion
    
    #region Variable Parsing
    
    /// <summary>
    /// Parses variable tokens
    /// </summary>
    protected Parser<VariableNode> Variable => VariableRule.Token().Named("Variable");

    /// <summary>
    /// Selects variable parser based on assignment style
    /// </summary>
    protected Parser<VariableNode> VariableRule => 
        _grammarConfig.AssignmentStyle == AssignmentStyle.Left ? LeftAssignmentParser : RightAssignmentParser;
    
    private Parser<VariableNode> LeftAssignmentParser =>
        from id in Identifier.Commented(this)
        from expr in (from _ in Parse.String(_grammarConfig.Assign).Token()
            from e in Parse.Ref(() => Expr)
            select e).Optional()
        select new VariableNode { Name = id.Value, Expression = expr.GetOrDefault() };

    private Parser<VariableNode> RightAssignmentParser =>
        from expr in (from e in Parse.Ref(() => Expr)
            from _ in Parse.String(_grammarConfig.Assign).Token()
            select e)
        from id in Identifier.Commented(this)
        select new VariableNode { Name = id.Value, Expression = expr };
        
    /// <summary>
    /// Parses variable declarations
    /// </summary>
    protected Parser<VariableDeclarationNode> VariableDeclaration =>
        from vars in Variable.DelimitedBy(Parse.Char(',').Token())
        from _ in Parse.Char(';')
        select new VariableDeclarationNode { Variables = vars.ToList() };

    #endregion
    
    #region Common Parsers
    
    /// <summary>
    /// Parses keywords (case-insensitive)
    /// </summary>
    protected Parser<string> Keyword(string keyword) =>
        Parse.IgnoreCase(keyword)
            .Then(_ => Parse.LetterOrDigit.Or(Parse.Char('_')).Not())
            .Return(keyword);
    
    /// <summary>
    /// Parses expressions within parentheses
    /// </summary>
    protected Parser<ExpressionNode> ParenInnerExpression =>
        from _ in Parse.Char('(').Token()
        from expr in Expr.XOptional()
        from __ in Parse.Char(')').Token()
        select expr.GetOrDefault();

    /// <summary>
    /// Checks if text is a comment
    /// </summary>
    protected bool CompareComment(string text) =>
        text.StartsWith(_grammarConfig.SingleLineComment, StringComparison.Ordinal);
    
    /// <summary>
    /// Parses keyword expressions
    /// </summary>
    protected Parser<ExpressionNode> KeywordExpression(string keyword, bool withParen = false) =>
        from _ in Keyword(keyword).Token()
        from comments in CommentParser.AnyComment.Token().Many()
        from expr in (withParen ? ParenInnerExpression : Expr).XOptional()
        from __ in Parse.Char(';').Commented(this)
        select expr.GetOrDefault();

    #endregion
    
    #region Statement Parsing
    
    protected Parser<StatementNode> Statement =>
        from member in Block.Select(s => s as StatementNode)
            .Or(VariableDeclaration)
            .Or(ReturnStatement)
            .Or(InputStatement)
            .Or(OutputStatement)
            .Commented(this)
        select member.Value
            .WithLeadingComments(
                CreateComments(member.LeadingComments))
            .WithTrailingComments(
                CreateComments(member.TrailingComments));
    
    /// <summary>
    /// Parses return statements
    /// </summary>
    protected Parser<ReturnStatementNode> ReturnStatement =>
        from expr in KeywordExpression(_grammarConfig.Return)
        select new ReturnStatementNode { Expression = expr };

    /// <summary>
    /// Parses output statements
    /// </summary>
    protected Parser<OutputStatementNode> OutputStatement =>
        from expr in KeywordExpression(_grammarConfig.Output, true).Token()
        select new OutputStatementNode { Expression = expr };
        
    /// <summary>
    /// Parses input statements
    /// </summary>
    protected Parser<InputStatementNode> InputStatement =>
        from expr in KeywordExpression(_grammarConfig.Input, true).Token()
        select new InputStatementNode { Expression = expr };

    #endregion
    
    #region Method Parsing
    
    protected Parser<ParameterNode> FunctionDeclParamName =>
        from name in Identifier
        select new ParameterNode { Name = name };
    
    protected Parser<IEnumerable<ExpressionNode>> ParameterDeclarations =>
        FunctionDeclParamName.DelimitedBy(Parse.Char(',').Token());
        
    protected Parser<List<ExpressionNode>> MethodParameters =>
        from _ in Parse.Char('(').Token()
        from param in ParameterDeclarations.Optional()
        from __ in Parse.Char(')').Token()
        select param.GetOrElse([]).ToList();
        
    protected Parser<FunctionStatementNode> MethodBody =>
        from body in Block.Or(Parse.Char(';').Return(default(BlockStatementNode))).Token()
        select new FunctionStatementNode { BodyStatement = body };

    protected Parser<StatementNode> MethodDeclaration =>
        from _ in Keyword(_grammarConfig.Function)
        from comments in CommentParser.AnyComment.Token().Many()
        from pattern in MethodStyleDeclaration
        select pattern;

    protected Parser<StatementNode> MethodStyleDeclaration =>
        _grammarConfig.FunctionStyle == FunctionStyle.Postfix ? MethodLeftPosition : MethodRightPosition;
            
    protected Parser<StatementNode> MethodRightPosition =>
        from @params in MethodParameters
        from name in FunctionDeclParamName
        from member in MethodBody.Select(c => c as StatementNode)
        select member.WithName(name).WithParameters(@params);

    protected Parser<StatementNode> MethodLeftPosition =>
        from name in FunctionDeclParamName
        from @params in MethodParameters
        from member in MethodBody.Select(c => c as StatementNode)
        select member.WithName(name).WithParameters(@params);

    protected Parser<StatementNode> IncludeDeclaration =>
        from _ in Keyword(_grammarConfig.Include)
        from comments in CommentParser.AnyComment.Token().Many()
        from files in StringLiteral.DelimitedBy(Parse.Char(',').Token())
        from __ in Parse.Char(';').Token()
        select new ImportStatementNode { Files = files.ToList() };

    #endregion
    
    #region Scope Parsing

    protected Parser<BlockStatementNode> Block =>
        from comments in CommentParser.AnyComment.Token().Many()
        from leading in Parse.Char('{').Token()
        from statements in Statement.Many()
        from trailing in Parse.Char('}').Commented(this)
        select new BlockStatementNode
        {
            BlockMember = statements.ToList(),
            InnerComments = trailing.LeadingComments.ToList(),
            TrailingComments = CreateComments(trailing.TrailingComments).ToList(),
        };
        
    protected Parser<StatementNode> MemberCodeDeclaration =>
        from comments in CommentParser.AnyComment.Token().Many()
        from member in MethodDeclaration
            .Or(IncludeDeclaration)
            .Or(VariableDeclaration)
            .Or(InputStatement)
            .Or(OutputStatement)
        select member.WithLeadingComments(CreateComments(comments));

    protected Parser<ListStatementNode> MemberListDeclaration =>
        from members in MemberCodeDeclaration.Many()
        select new ListStatementNode { Members = members.ToList() };


    #endregion
    
    #region Expression parsing
    protected Parser<ExpressionType> Operator(string op, ExpressionType opType) =>
        Parse.String(op).Token().Return(opType);

    protected Parser<ExpressionType> Add => Operator(_grammarConfig.Plus, ExpressionType.AddChecked);
    protected Parser<ExpressionType> Subtract => Operator(_grammarConfig.Minus, ExpressionType.SubtractChecked);
    protected Parser<ExpressionType> Multiply => Operator(_grammarConfig.Multiply, ExpressionType.MultiplyChecked);
    protected Parser<ExpressionType> Divide => Operator(_grammarConfig.Divide, ExpressionType.Divide);
    protected Parser<ExpressionType> Modulo => Operator(_grammarConfig.Modulo, ExpressionType.Modulo);
    protected Parser<ExpressionType> Power => Operator(_grammarConfig.Power, ExpressionType.Power);
    protected Parser<ExpressionType> Not => Operator(_grammarConfig.Not, ExpressionType.Not);
    protected Parser<ExpressionType> Negate => Operator(_grammarConfig.Minus, ExpressionType.Negate);
    
    protected Parser<ExpressionNode> Parameter => Parse.Ref(() => Expr);
    protected Parser<IEnumerable<ExpressionNode>> ParameterExpr => Parameter.DelimitedBy(Parse.Char(',').Token());
    
    protected Parser<List<ExpressionNode>> MethodParametersExpr =>
        from _ in Parse.Char('(')
        from param in ParameterExpr.Optional()
        from __ in Parse.Char(')')
        select param.GetOrElse([]).ToList();
        
    protected Parser<FunctionStatementNode> MethodRightExpression =>
        from @params in MethodParametersExpr
        from name in Identifier
        select new FunctionStatementNode { Name = name, Parameters = @params };
    
    protected Parser<FunctionStatementNode> MethodLeftExpression =>
        from name in Identifier
        from @params in MethodParametersExpr
        select new FunctionStatementNode { Name = name, Parameters = @params };
    
    protected Parser<FunctionStatementNode> MethodExpression =>
        _grammarConfig.FunctionStyle == FunctionStyle.Postfix ? MethodLeftExpression : MethodRightExpression;

    protected Parser<InputStatementNode> InputExpression =>
        from _ in Keyword(_grammarConfig.Input).Token()
        from comments in CommentParser.AnyComment.Token().Many()
        from expr in ParenInnerExpression
        select new InputStatementNode
        {
            Expression = expr
        };

    protected Parser<VariableNode> VariableExpression =>
        from id in Identifier.Commented(this)
        select new VariableNode { Name = id.Value };

    protected Parser<UnaryOperationNode> NotExpression =>
        from not in Not
        from comments in CommentParser.AnyComment.Token().Many()
        from expr in ParenInnerExpression
        select ExpressionNode.CreateUnary(expr, not);
    
    protected Parser<ExpressionNode> Factor =>
        (from _ in Parse.Char('(')
         from expr in Parse.Ref(() => Expr)
         from __ in Parse.Char(')')
         select expr)
        .Or(InputExpression)
        .Or(NotExpression)
        .Or(MethodExpression)
        .Or(VariableExpression)
        .Or(LiteralExpression)
        .Named("expression");

    protected Parser<ExpressionNode> Operand =>
        from operand in 
            (from op in Negate
                from factor in Factor
                select ExpressionNode.CreateUnary(factor, op))
                .XOr(Factor)
                .Token()
            .Commented(this)
        select operand.Value;

    protected Parser<ExpressionNode> InnerTerm =>
        Parse.ChainRightOperator(Power, Operand, MakeBinary);

    protected Parser<ExpressionNode> Term =>
        Parse.ChainOperator(Multiply.Or(Divide).Or(Modulo), InnerTerm, MakeBinary);

    public Parser<ExpressionNode> Expr =>
        Parse.ChainOperator(Add.Or(Subtract), Term, MakeBinary);

    protected BinaryOperationNode MakeBinary(ExpressionType op, ExpressionNode left, ExpressionNode right) =>
        new BinaryOperationNode { Operator = op, Left = left, Right = right };

    #endregion
   
    /// <summary>
    /// Root parser for AST nodes
    /// </summary>
    public Parser<StatementNode> AstNodeExpression =>
        from member in MemberListDeclaration.Select(c => c as StatementNode)
        from _ in Parse.WhiteSpace.Many()
        from trailing in CommentParser.AnyComment.Token().Many().End()
        select member.WithTrailingComments(CreateComments(trailing));
    
    #endregion
    
}