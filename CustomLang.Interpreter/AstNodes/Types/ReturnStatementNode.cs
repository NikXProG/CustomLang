namespace CustomLang.Interpreter.AstNodes.Types;

public class ReturnStatementNode : StatementNode
{
    public override AstNodeType Kind => AstNodeType.ReturnStatementNode;

    public override void Accept(CustomLangVisitor visitor) => visitor.VisitReturnStatement(this);
    public ExpressionNode Expression { get; set; }
}