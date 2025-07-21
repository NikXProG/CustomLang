namespace CustomLang.Interpreter.AstNodes.Types;

public class FunctionStatementNode : StatementNode
{
    public override AstNodeType Kind => AstNodeType.MethodNode;
    
    public override void Accept(CustomLangVisitor visitor) => 
        visitor.VisitMethodExpression(this);
    
    public TypeNode ReturnType { get; set; }
    public string Name { get; set; }
    
    public List<ExpressionNode> Parameters { get; set; } = new List<ExpressionNode>();
    
    public override StatementNode WithName(ParameterNode typeAndName)
    {
        // ReturnType = typeAndName.Type;
        Name = typeAndName.Name ?? typeAndName.Type.Name;
        return this;
    }

    public override StatementNode WithParameters(List<ExpressionNode> parameters)
    {
        Parameters = parameters;
        return this;
    }
    public BlockStatementNode? BodyStatement { get; set; }

    public bool IsEmpty => BodyStatement == null;
    
  
}