namespace CustomLang.Interpreter.AstNodes.Types;

public class StatementNode : ExpressionNode
{
    public StatementNode(string body = null)
    {
        Body = body;
    }
    
    public override AstNodeType Kind => AstNodeType.StatementNode;
    public override void Accept(CustomLangVisitor visitor) => visitor.VisitStatement(this);
    
    public string Body { get; set; }
    
    public virtual StatementNode WithName(ParameterNode Name)
    {
        return this;
    }
    
    public virtual StatementNode WithParameters(List<ExpressionNode> parameters)
    {
        return this;
    }
    
}