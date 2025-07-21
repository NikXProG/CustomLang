using System.Globalization;
using System.Linq.Expressions;
using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.Exceptions;
using CustomLang.Interpreter.Extentions;
using CustomLang.Interpreter.Visitors.LangObj;
using Double = CustomLang.Interpreter.Visitors.LangObj.Double;
using String = CustomLang.Interpreter.Visitors.LangObj.String;

namespace CustomLang.Interpreter.Visitors;

public partial class InterpreterCodeVisitor
{
    
    public override void VisitVariable(VariableNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
    
        if (node.Expression != null)
        {
            node.Expression.Accept(this);
            _context.VariablesStorage[node.Name] = PopValue(nameof(VariableNode));
        }
        else if (_context.VariablesStorage.TryGetValue(node.Name, out var value))
        {
            _stackValues.Push(value);
        }
        else
        {
            throw new InvalidOperationException($"Variable {node.Name} not defined");
        }
        
        base.VisitVariable(node);
 
    }



    public override void VisitBinaryExpression(BinaryOperationNode node)
    {
        ArgumentNullException.ThrowIfNull(node);

        node.Left.Accept(this);
        node.Right.Accept(this);

        var right = PopValue(nameof(BinaryOperationNode.Right));
        var left = PopValue(nameof(BinaryOperationNode.Left));

        try
        {
            var result = CalculateBinaryOperation(node.Operator, left, right);
            _stackValues.Push(result);
        }
        catch (OverflowException ex)
        {
            throw new InvalidOperationException(
                $"Arithmetic overflow when applying operator {node.Operator} to {left} and {right}", ex);
        }
        catch (DivideByZeroException ex)
        {
            throw new InvalidOperationException(
                $"Division by zero when applying operator {node.Operator} to {left} and {right}", ex);
        }
        catch (UncertaintyException ex)
        {
            throw new InvalidOperationException(
                $"Uncertainty arises when {node.Operator} to {left} and {right}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Error applying operator {node.Operator} to {left} and {right}", ex);
        }
    }
    private LangObject CalculateBinaryOperation(ExpressionType op, LangObject left, LangObject right)
    {
        
        //obsolete code
         if (op == ExpressionType.AddChecked)
         {
             if (left is String leftStr)
            {
                 return new String(leftStr + right.ToString());
             }
             if (right is String rightStr)
             {
                 return new String(left.ToString() + rightStr);
             }
         }
        
        var leftNum = left.ToNumeric();
        var rightNum = right.ToNumeric();

        return op switch
        {
            ExpressionType.AddChecked => leftNum + rightNum,
            ExpressionType.SubtractChecked => leftNum - rightNum,
            ExpressionType.MultiplyChecked => leftNum * rightNum,
            ExpressionType.Divide => leftNum / rightNum,
            ExpressionType.Modulo => leftNum % rightNum,
            ExpressionType.Power => leftNum.Pow(rightNum),
            _ => throw new NotSupportedException($"Operator {op} not supported")
        };
    }
    

    public override void VisitSystemTypeExpression(TokenTypeExpressionNode node)
    { 
        ArgumentNullException.ThrowIfNull(node);

        if (node.Token == null)
        {
            throw new InvalidOperationException("Literal value cannot be null");
        }
        
        
       
        switch (node.LiteralType)
        {
            case SystemType.Integer:
                //obsolete code
                _stackValues.Push(
                    node.Token
                        .ToBase(_config.AssignmentBase, 10)
                        .ParseInt());
                break;
            case SystemType.Float:
                _stackValues.Push(new Double(
                    Convert.ToDouble(node.Token.DecimalToBase(_config.AssignmentBase, 10),
                        CultureInfo.InvariantCulture)));
                break;
            case SystemType.String:
                _stackValues.Push(new String(node.Token));
                break;
            default:
                throw new NotSupportedException($"Literal type {node.LiteralType} is not supported");
        }
        
        base.VisitSystemTypeExpression(node);
    }

    
    public override void VisitUnaryExpression(UnaryOperationNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
      
        node.Operand.Accept(this);
       
        var operand =  PopValue(nameof(UnaryOperationNode.Operand));

        var num = operand.ToNumeric();
        
        _stackValues.Push(node.Operator switch
        {
            ExpressionType.Not => ~num,
            ExpressionType.Negate => -num,
            _ => throw new NotSupportedException($"Operator {node.Operator} not supported")
        });
     
        
        base.VisitUnaryExpression(node);
    }

    public override void VisitParameter(ParameterNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        
        base.VisitParameter(node);
    }

    
}