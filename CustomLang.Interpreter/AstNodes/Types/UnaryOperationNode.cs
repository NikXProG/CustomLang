using System.Linq.Expressions;

namespace CustomLang.Interpreter.AstNodes.Types;

public class UnaryOperationNode : ExpressionNode
{
    public override void Accept(CustomLangVisitor visitor) =>
        visitor.VisitUnaryExpression(this);
    
    public ExpressionType Operator { get; set; }
    public ExpressionNode Operand { get; set; }
    
}