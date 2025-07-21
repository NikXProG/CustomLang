using System.Linq.Expressions;

namespace CustomLang.Interpreter.AstNodes.Types;

public class BinaryOperationNode : ExpressionNode
{
    
    public override void Accept(CustomLangVisitor visitor) =>
        visitor.VisitBinaryExpression(this);
    public override AstNodeType Kind => AstNodeType.BinaryExpressionNode;

    public ExpressionNode Left { get; set; }

    public ExpressionType Operator { get; set; }

    public  ExpressionNode Right { get; set; }
    
}