using System.Linq.Expressions;
using CustomLang.Interpreter.AstNodes;
using CustomLang.Interpreter.AstNodes.Types;

namespace CustomLang.Interpreter;

public abstract class CustomLangVisitor
{
    public virtual void DefaultVisit(AstNode node)
    {
      
    }
    
    public virtual void VisitVariable(VariableNode node) => DefaultVisit(node);
    public virtual void VisitExpression(ExpressionNode node) => DefaultVisit(node);
    public virtual void VisitType(TypeNode node) => DefaultVisit(node);
    public virtual void VisitParameter(ParameterNode node) => DefaultVisit(node);
    public virtual void VisitVariableDeclaration(VariableDeclarationNode node) => DefaultVisit(node);
    public virtual void VisitSystemTypeExpression(TokenTypeExpressionNode node) => DefaultVisit(node);
    public virtual void VisitUnaryExpression(UnaryOperationNode node) => DefaultVisit(node);
    public virtual void VisitMethodExpression(FunctionStatementNode statementNode) => DefaultVisit(statementNode);
    public virtual void VisitBinaryExpression(BinaryOperationNode node) => DefaultVisit(node);
    public virtual void VisitReturnStatement(ReturnStatementNode node) => DefaultVisit(node);
    public virtual void VisitStatement(StatementNode node) => DefaultVisit(node);
    public virtual void VisitBlockStatement(BlockStatementNode statementNode) => DefaultVisit(statementNode);
    public virtual void VisitImportStatement(ImportStatementNode node) => DefaultVisit(node);
    public virtual void VisitOutputStatement(OutputStatementNode node) => DefaultVisit(node);
    public virtual void VisitInputStatement(InputStatementNode node) => DefaultVisit(node);
    public virtual void VisitListStatement(ListStatementNode node) => DefaultVisit(node);

}