using System.Linq.Expressions;
using Sprache;

namespace CustomLang.Interpreter.AstNodes.Types;


public class ExpressionNode : AstNode
{

    public ExpressionNode() { }
    
    public override void Accept(CustomLangVisitor visitor) =>
        visitor.VisitExpression(this);
    
    public ExpressionNode(string expr)
    {
        ExpressionString = expr;
    }
    
    public static ExpressionNode? CreateOrDefault(IOption<string> expression)
    {
        return expression.IsDefined ? new ExpressionNode(expression.Get()) : null;
    }
    
    public override AstNodeType Kind => AstNodeType.ExpressionNode;
    private string ExpressionString { get; set; }
    
    public override string? ToString()
    {
        return !string.IsNullOrEmpty(ExpressionString) ? ExpressionString : base.ToString();
    }

    public static UnaryOperationNode CreateUnary(ExpressionNode expression, ExpressionType typeOp)
    {
        return new UnaryOperationNode
        {
            Operand = expression,
            Operator = typeOp
        };
    }
    


}