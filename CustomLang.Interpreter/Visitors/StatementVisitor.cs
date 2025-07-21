using System.Text;
using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.Config;
using CustomLang.Interpreter.Extentions;
using CustomLang.Interpreter.Visitors.LangObj;
using CustomLang.Storage.Trie;

namespace CustomLang.Interpreter.Visitors;

public partial class InterpreterCodeVisitor
{
    public override void VisitImportStatement(ImportStatementNode node)
    {
        
        ArgumentNullException.ThrowIfNull(node);
        
        foreach (var  file in node.Files)
        {
            var fileNoLiteral= file.Trim('\'');
            
            var baseDirectory = Path.GetDirectoryName(fileNoLiteral) ?? Directory.GetCurrentDirectory();
            
            var fullPath = Path.IsPathRooted(fileNoLiteral) 
                ? fileNoLiteral
                : Path.Combine(baseDirectory, fileNoLiteral);
            
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File not found - {fullPath}");
            }
                
            var builder = new StringBuilder();
            
            using (var reader = new StreamReader(fullPath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    builder.AppendLine(line);
                }
            }
            
            var function = _parser.Parse(builder.ToString());
                
            function.Accept(this);

        }
        
        base.VisitImportStatement(node);
    }
    
    
    public override void VisitMethodExpression(FunctionStatementNode statementNode)
    {
        ArgumentNullException.ThrowIfNull(statementNode);
        
        StartDebug(statementNode.LeadingComments);
        StartDebug(statementNode.TrailingComments);
        
        
        if (!_context.FunctionsStorage.TryGetValue(statementNode.Name, out var value))
        {
            _context.FunctionsStorage[statementNode.Name] = statementNode;
            return;
        }
        
        if (value == null){
            throw new NullReferenceException($"Not found function with name {statementNode.Name}");
        }
        
        foreach (var arg in statementNode.Parameters)
        {
            arg.Accept(this);
        }

        var scope = _context.VariablesStorage.Clone();
        scope.Clear();
        
        if (value is FunctionStatementNode method)
        {
            for (int i = method.Parameters.Count - 1; i >= 0; i--)
            {
                
                var name = (method.Parameters[i] as ParameterNode)?.Name;
                if (name != null)  
                {
                    scope[name] = PopValue(nameof(FunctionStatementNode.Parameters));
                }
                
            }
            
            var oldVariables = _context.VariablesStorage.Clone();
            
            foreach (var kvp in scope)
            {
                _context.VariablesStorage[kvp.Key] = kvp.Value;
            }
            
            method.BodyStatement?.Accept(this);
            
            _context.VariablesStorage.Clear();
            
            foreach (var kvp in oldVariables)
            {
                _context.VariablesStorage[kvp.Key] = kvp.Value;
            }
            
        
        }
        
        base.VisitMethodExpression(statementNode);
    }

    public override void VisitVariableDeclaration(VariableDeclarationNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        
        StartDebug(node.LeadingComments);
        StartDebug(node.TrailingComments);

        
        foreach (var t in node.Variables)
        {
            t.Accept(this);
        }
  
        base.VisitVariableDeclaration(node);
    }
    
    public override void VisitReturnStatement(ReturnStatementNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        
        StartDebug(node.LeadingComments);
        StartDebug(node.TrailingComments);


        node.Expression.Accept(this);
        base.VisitReturnStatement(node);
    }
    
    
    public override void VisitOutputStatement(OutputStatementNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        
        StartDebug(node.LeadingComments);
        StartDebug(node.TrailingComments);

        if (node.Expression == null)
        {
            _ioContext.OutputWriter.WriteLine();
            return;
        }
        
        node.Expression?.Accept(this);

        var obj = PopValue(nameof(OutputStatementNode));
        
        var value = obj.ToString();
        
        if (obj.GetObjType() == LangType.Int32)
        {
            value = value?.ToBase(10, _config.OutputBase);
        }

        if (obj.GetObjType() == LangType.Double)
        {
            value = value?.DecimalToBase(10, _config.OutputBase);
        }

        if (obj.GetObjType() == LangType.String)
        {
            value = ((LangObj.String)obj).ToString();
        }
      
        _ioContext.OutputWriter.WriteLine(value);

        base.VisitOutputStatement(node);
    }
    
    public override void VisitBlockStatement(BlockStatementNode statementNode)
    {
        ArgumentNullException.ThrowIfNull(statementNode);
        
        foreach (var member in statementNode.BlockMember)
        {
            member.Accept(this);
        }
        base.VisitBlockStatement(statementNode);
    }
    
    public override void VisitInputStatement(InputStatementNode node)
    {
        
        ArgumentNullException.ThrowIfNull(node);
        
        node.Expression?.Accept(this);

        int baseInput = _config.InputBase;
        
        if (node.Expression != null)
        {
            int.TryParse(PopValue(nameof(InputStatementNode)).ToString(), out var result);
            baseInput = result;
        }
        
        var input = _ioContext.InputReader.ReadLine();
      
        if (!string.IsNullOrEmpty(input))
        {
            var convertedValue = input.FromBase(baseInput);
            
            _stackValues.Push(new Long(convertedValue));
        }
      
        base.VisitInputStatement(node);
    }

    public override void VisitListStatement(ListStatementNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        
        foreach (var memberCode in  node.Members)
        {
            memberCode.Accept(this);
        }
        
        StartDebug(node.TrailingComments);
        
        base.VisitListStatement(node);
        
    }
    
}