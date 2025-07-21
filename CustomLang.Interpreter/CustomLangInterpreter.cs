using System.Linq.Expressions;
using System.Text;
using CustomLang.Interpreter.interfaces;
using CustomLang.Interpreter.AstNodes;
using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.config;
using CustomLang.Interpreter.Config;
using CustomLang.Interpreter.Extentions;
using CustomLang.Interpreter.Visitors;
using CustomLang.Storage;
using CustomLang.Storage.Trie;

namespace CustomLang.Interpreter;

public class CustomLangInterpreter : ICodeInterpreter
{

    #region Fields

    private readonly CustomLangVisitor _visitor;
    private readonly ICustomLangParser _parser;

    #endregion
  
    #region Constructors
    
    public CustomLangInterpreter(
        ICustomLangParser parser,
        CustomLangVisitor visitor)
    {
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
    }
    
    #endregion
    
    #region Methods

    public async Task ParseAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or whitespace", nameof(code));
        
        var node = await Task.Run(() => _parser.Parse(code), cancellationToken)
            .ConfigureAwait(false);
        node.Accept(_visitor);
    }
    
    public void Parse(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or whitespace", nameof(code));
        
        var node = _parser.Parse(code);
        node.Accept(_visitor);
    }
    
    
    #endregion
    
}
