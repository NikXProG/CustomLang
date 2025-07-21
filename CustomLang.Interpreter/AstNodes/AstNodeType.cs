namespace CustomLang.Interpreter.AstNodes;

public enum AstNodeType
{
    UnknownNode,
    ExpressionNode,
    VariableNode,
    BinaryExpressionNode,
    TypeNode,
    MethodNode,
    ParameterNode,
    VariableDeclarationNode,
    StatementNode,
    SystemTypeExpressionNode,
    ReturnStatementNode,
    BlockBodyStatementNode,
    OutputStatementNode,
    NotStatementNode,
    InputStatementNode,
    ImportStatementNode,
    ListStatementNode,
}