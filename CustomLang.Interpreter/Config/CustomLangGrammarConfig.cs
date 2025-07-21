using System.Reflection;
using CustomLang.Config.Parser;
using CustomLang.Interpreter.interfaces;
using CustomLang.Interpreter.AstNodes.Types;

namespace CustomLang.Interpreter.Config;



public enum AssignmentStyle
{
    Left,
    Right
}

public enum FunctionStyle
{
    Prefix,
    Postfix
}

public enum UnaryOperationStyle
{
    Infix,
    Postfix
}

public class CustomLangGrammarConfig 
{
    
    #region Constants
    
    private const string SyntaxSettings = "SyntaxSettings";

    private const string OperationSettings = "OperationSettings";
    
    private const string CommentSettings = "CommentSettings";
    
    private const string StyleSettings = "StyleSettings";
    
    #endregion
    
    #region Constructors
    
    public CustomLangGrammarConfig()
    {
        
    }
    
    #endregion
    
    #region Properties
    
    #region Grammar Properties
    
    #region Syntax
    
    [IniProperty(SyntaxSettings, "Float")]
    public string Float { get; set; } = "float";
    
    [IniProperty(SyntaxSettings, "Void")]
    public string Void { get; set; } = "void";
    
    [IniProperty(SyntaxSettings, "Int")]
    public string Int { get; set; } = "int";
    
    [IniProperty(SyntaxSettings, "String")]
    public string String { get; set; } = "string";
    
    [IniProperty(SyntaxSettings, "Return")]
    public string Return { get; set; } = "return";
    
    [IniProperty(SyntaxSettings, "Output")]
    public string Output { get; set; } = "output";
    
    [IniProperty(SyntaxSettings, "Input")]
    public string Input { get; set; } = "input";
    
    [IniProperty(SyntaxSettings, "Not")]
    public string Not { get; set; } = "not";
    
    [IniProperty(SyntaxSettings, "Function")]
    public string Function { get; set; } = "function";
    
    [IniProperty(SyntaxSettings, "Use")]
    public string Include { get; set; } = "use";

    #endregion
    
    #region Syntax Style
    
    [IniProperty(StyleSettings, "Direction")]
    public AssignmentStyle AssignmentStyle { get; set; } = AssignmentStyle.Left;

    [IniProperty(StyleSettings, "FunctionStyle")]
    public FunctionStyle FunctionStyle { get; set; } = FunctionStyle.Postfix;
    
    [IniProperty(StyleSettings, "UnaryOperationStyle")]
    public UnaryOperationStyle UnaryOperationStyle { get; set; } = UnaryOperationStyle.Postfix;
    
    #endregion
    
    #region Operations
    
    [IniProperty(OperationSettings, "Add")]
    public string Plus { get; set; } = "+";
    
    [IniProperty(OperationSettings, "Sub")]
    public string Minus { get; set; } = "-";
    
    [IniProperty(OperationSettings, "Mult")]
    public string Multiply { get; set; } = "*";
    
    [IniProperty(OperationSettings, "Div")]
    public string Divide { get; set; } = "/";
    
    [IniProperty(OperationSettings, "Mod")]
    public string Modulo { get; set; } = "%";
    
    [IniProperty(OperationSettings, "Pow")]
    public string Power { get; set; } = "^";
    
    [IniProperty(OperationSettings, "Assign")]
    public string Assign { get; set; } = "=";
    
    #endregion
        
    #region Comments
    
    [IniProperty(CommentSettings, "Single")]
    public string SingleLineComment { get; set; } = "#";
    
    [IniProperty(CommentSettings, "MultiOpen")]
    public string MultiLineOpenComment { get; set; } = "/*";
    
    [IniProperty(CommentSettings, "MultiClose")]
    public string MultiLineCloseComment { get; set; } = "*/";
    
    [IniProperty(CommentSettings, "NewLine")]
    public string NewLineComment { get; set; } = "\n";
    
    #endregion
    
    #endregion
    
    public HashSet<string> ReservedWords { get; } =
        GetStrings(AllStringConstants.ToArray());
    
    private static IEnumerable<string> AllStringConstants =>
        typeof(CustomLangGrammarConfig).GetTypeInfo().GetFields()
            .Select(f => f.GetValue(null)).OfType<string>();
    
    #endregion
    
    #region Methods
    
    private static HashSet<string> GetStrings(params string[] strings) 
        => new(strings, StringComparer.OrdinalIgnoreCase);

    
    public void Configure(Action<KeywordConfiguration> configurator)
    {
        ArgumentNullException.ThrowIfNull(configurator);
        var config = new KeywordConfiguration(this);
        configurator(config);
    }
    
    #endregion
    
}