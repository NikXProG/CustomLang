using System.Linq.Expressions;
using CustomLang.Interpreter.AstNodes.Types;
using Sprache;

namespace CustomLang.Interpreter.AstNodes;



public abstract class AstNode
{
    public abstract AstNodeType Kind { get; }
  
    public abstract void Accept(CustomLangVisitor visitor);
    
    public List<Comment> LeadingComments { get; set; } = new List<Comment>();
    
    public List<Comment> TrailingComments { get; set; } = new List<Comment>();
    
}


//
// public abstract class AstNode { }
//
// public abstract class Expr : AstNode
// {
//     public abstract uint Evaluate(IVariableContext context);
// }
//
// public abstract class Statement : AstNode
// {
//     public abstract void Execute(IVariableContext context);
// }
//
//
// public class NumberLiteral : Expr
// {
//     public uint Value { get; }
//     
//     public NumberLiteral(uint value) => Value = value;
//     
//     public override uint Evaluate(IVariableContext _) => Value;
// }
//
// public class VariableReference : Expr
// {
//     public string Name { get; }
//     
//     public VariableReference(string name) => Name = name;
//     
//     public override uint Evaluate(IVariableContext context) => 
//         context.GetVariable(Name);
// }
//
//
// public class AssignmentStatement : Statement
// {
//     public string VariableName { get; }
//     public Expr Expr { get; }
//     
//     public override void Execute(IVariableContext context)
//     {
//         context.SetVariable(VariableName, Expr.Evaluate(context));
//     }
// }
//
// public class BinaryOperation : Expr
// {
//     public Expr Left { get; }
//     
//     public string Operator { get; }
//     public Expr Right { get; }
//
//     public override uint Evaluate(IVariableContext context) =>
//         Left.Evaluate(context) + Right.Evaluate(context);
// }
//
//
// public class UnaryOperation : Expr
// {
//     public string Operator { get; }
//     public Expr Expr { get; }
//     
//     public UnaryOperation(string op, Expr expr)
//     {
//         Operator = op;
//         Expr = expr;
//     }
//     
//     public override uint Evaluate(IVariableContext context)
//     {
//         var value = Expr.Evaluate(context);
//         
//         switch (Operator)
//         {
//             case "not":
//                 return ~value;
//             case "input":
//                 Console.Write("Enter value: ");
//                 var input = Console.ReadLine();
//                 return Convert.ToUInt32(input, 10);
//             default:
//                 throw new InvalidOperationException($"Unknown unary operator: {Operator}");
//         }
//     }
// }
//
// public class OutputStatement : Statement
// {
//     public Expr Expr { get; }
//     
//     public OutputStatement(Expr expr)
//     {
//         Expr = expr;
//     }
//     
//     public override void Execute(IVariableContext context)
//     {
//         var value = Expr.Evaluate(context);
//         Console.WriteLine(value.ToString());
//     }
// }








// public class VariableExpr : Expr 
// {
//     public string Name { get; }
//     public VariableExpr(string name) => Name = name;
// }
//
//
// public class ConstantExpr : Expr 
// {
//     public uint Value { get; }
//     public ConstantExpr(uint value) => Value = value;
// }
// public class UnaryOpExpr : Expr 
// {
//     public string Op { get; }
//     public Expr Argument { get; }
//     public UnaryOpExpr(string op, Expr arg) => (Op, Argument) = (op, arg);
// }
// public class BinaryOpExpr : Expr 
// {
//     public string Op { get; }
//     public Expr Left { get; }
//     public Expr Right { get; }
//     public BinaryOpExpr(string op, Expr left, Expr right) => (Op, Left, Right) = (op, left, right);
// }
