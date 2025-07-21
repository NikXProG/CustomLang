using System.Collections;

namespace CustomLang.Interpreter.AstNodes.Types;

public class BlockStatementNode :
    StatementNode,
    IEnumerable, IEnumerable<StatementNode>
{
    public override AstNodeType Kind => AstNodeType.BlockBodyStatementNode;
    public override void Accept(CustomLangVisitor visitor) => visitor.VisitBlockStatement(this);
    public List<StatementNode> BlockMember { get; set; } = new List<StatementNode>();

    public void Add(StatementNode statement) => BlockMember.Add(statement);
    
    public List<string> InnerComments { get; set; } = new List<string>();    
    
    public IEnumerator GetEnumerator() => ((IEnumerable)BlockMember).GetEnumerator();

    IEnumerator<StatementNode> IEnumerable<StatementNode>.GetEnumerator() => BlockMember.GetEnumerator();
}