using System.Linq.Expressions;

namespace CustomLang.Interpreter.AstNodes.Types;

public class ParameterNode : ExpressionNode
{
    public override void Accept(CustomLangVisitor visitor) 
        => visitor.VisitParameter(this);

    public override AstNodeType Kind => AstNodeType.ParameterNode;

    public string Name { get; init; }
    
    public TypeNode Type { get; set; }

}