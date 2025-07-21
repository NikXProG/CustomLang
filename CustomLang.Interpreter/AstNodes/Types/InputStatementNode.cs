namespace CustomLang.Interpreter.AstNodes.Types;

public class InputStatementNode : StatementNode
{
    public override AstNodeType Kind => AstNodeType.InputStatementNode;
    public override void Accept(CustomLangVisitor visitor) => 
        visitor.VisitInputStatement(this);

    public ExpressionNode Expression { get; set; }
    
}