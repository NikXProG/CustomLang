namespace CustomLang.Interpreter.AstNodes.Types;

public class OutputStatementNode  : StatementNode
{
    public override AstNodeType Kind => AstNodeType.OutputStatementNode;
    public override void Accept(CustomLangVisitor visitor) => visitor.VisitOutputStatement(this);
    
    public ExpressionNode Expression { get; set; }
    public string? Format { get; set; }
}