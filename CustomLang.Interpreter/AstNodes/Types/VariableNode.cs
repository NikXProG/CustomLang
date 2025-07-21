namespace CustomLang.Interpreter.AstNodes.Types;

public class VariableNode : ExpressionNode
{
    public override void Accept(CustomLangVisitor visitor) =>
        visitor.VisitVariable(this);
    public string Name { get; set;  }
    public  ExpressionNode Expression { get; set; }
   
    public override AstNodeType Kind => AstNodeType.VariableNode;
    
}