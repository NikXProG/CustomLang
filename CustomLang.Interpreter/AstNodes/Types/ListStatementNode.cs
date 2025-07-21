namespace CustomLang.Interpreter.AstNodes.Types;

public class ListStatementNode : StatementNode
{

    public List<StatementNode> Members { get; set; } = new List<StatementNode>();

    public override AstNodeType Kind => AstNodeType.ListStatementNode;
    public override void Accept(CustomLangVisitor visitor) => visitor.VisitListStatement(this);

}