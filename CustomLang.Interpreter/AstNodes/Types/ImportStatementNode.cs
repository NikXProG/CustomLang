namespace CustomLang.Interpreter.AstNodes.Types;

public class ImportStatementNode : StatementNode
{
    
    public override AstNodeType Kind => AstNodeType.ImportStatementNode;
    
    public override void Accept(CustomLangVisitor visitor) => visitor.VisitImportStatement(this);

    public List<string> Files = new List<string>();
    
}