namespace CustomLang.Interpreter.AstNodes.Types;

public class VariableDeclarationNode  : StatementNode
{
    public override AstNodeType Kind => AstNodeType.VariableDeclarationNode;
    
    public override void Accept(CustomLangVisitor visitor) => visitor.VisitVariableDeclaration(this);
    
    public TypeNode Type { get; set; }
    public List<VariableNode> Variables { get; set; } = new List<VariableNode>();
    
}