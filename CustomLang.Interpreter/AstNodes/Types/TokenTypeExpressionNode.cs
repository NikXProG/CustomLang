namespace CustomLang.Interpreter.AstNodes.Types;

public class TokenTypeExpressionNode : ExpressionNode
{
    public override void Accept(CustomLangVisitor visitor) =>
        visitor.VisitSystemTypeExpression(this);
    public TokenTypeExpressionNode(string token = null,
        SystemType type = SystemType.Null)
    {
        Token = token;
        LiteralType = type;
    }
    
    public string Token { get; set; }
    public SystemType LiteralType { get; set; }
    
    public override AstNodeType Kind => AstNodeType.SystemTypeExpressionNode;
    
}