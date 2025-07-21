using System.Text.RegularExpressions;
using CustomLang.Interpreter.AstNodes;
using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.config;
using CustomLang.Interpreter.Debug;
using CustomLang.Interpreter.Exceptions;
using CustomLang.Interpreter.interfaces;
using CustomLang.Interpreter.Extentions;
using CustomLang.Interpreter.Visitors.LangObj;
using CustomLang.Storage;
using CustomLang.Storage.Trie;

namespace CustomLang.Interpreter.Visitors;

public partial class InterpreterCodeVisitor : CustomLangVisitor
{
    private readonly IStorageContext _context;
    
    private readonly IIOContext _ioContext;

    private readonly ICustomLangParser _parser;

    private readonly InterpreterConfig _config;
    
    private readonly ICustomLangDebug? _debug;
    
    private readonly Stack<LangObject> _stackValues;
    
    public InterpreterCodeVisitor(
        ICustomLangParser parser,
        IStorageContext context,
        IIOContext ioContext,
        InterpreterConfig config,
        StatementNode? functionsList = null,
        ICustomLangDebug? debug = null)
    {
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _ioContext = ioContext ?? throw new ArgumentNullException(nameof(ioContext));
        
        if (_config.DebugMode && debug is null)
        {
            throw new InvalidOperationException(
                "Debug mode is enabled but debugger was not provided (debugger is null)");
        }

        _stackValues = new Stack<LangObject>();
        
        _debug = debug;
        
        functionsList?.Accept(this);
        
    }

    private LangObject PopValue(string contextCall)
    {
        if ( _stackValues.Count == 0)
            throw new InvalidOperationException(
                $"Stack underflow in context: {contextCall}");
        return _stackValues.Pop();
    }

    private LangObject PeekValue(string contextCall)
    {
        if ( _stackValues.Count == 0)
            throw new InvalidOperationException(
                $"Stack underflow in context: {contextCall}");
        return  _stackValues.Peek();
    }
    
    public override void DefaultVisit(AstNode node)
    {
        base.DefaultVisit(node);
    }
    
    
    private void StartDebug(IEnumerable<Comment> comments)
    {
        if (!_config.DebugMode || _debug == null)
        {
            return;
        }
        
        
        foreach (var comment in comments)
        {
            if (!comment.IsSingle) continue;

            if (!MyRegex().IsMatch(comment.Text)) continue;
            
            try
            {
                _debug.EnterInteractiveMode();
                
            }
            catch (DebugContinueException)
            {
                // skip...
            }

        }
        
    }

    [GeneratedRegex(@"^\s*BREAKPOINT\s*$")]
    private static partial Regex MyRegex();
    
}