namespace CustomLang.Config.Parser.Settings;

public class CommentIniParserSettings
{
    public List<CommentInfo> Comments { 
        get;
        set; 
    }
    
    /// <summary>
    /// Default parser settings with support for:
    /// - Single-line comments starting with #
    /// - Multi-line comments between /** and **/
    /// </summary>
    public static CommentIniParserSettings Default => new CommentIniParserSettings
    {
        Comments = new List<CommentInfo>
        {
            new CommentInfo { Start = "#" },
            new CommentInfo { Start = "/**", End = "**/" }
        }
    };
}