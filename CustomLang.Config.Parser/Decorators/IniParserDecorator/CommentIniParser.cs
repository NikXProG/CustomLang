using System.Text;
using System.Text.RegularExpressions;
using CustomLang.Config.Parser.Settings;
using IniParser;
using IniParser.Model;
using Microsoft.Extensions.Options;

namespace CustomLang.Config.Parser.Decorators.IniParserDecorator;

public partial class CommentIniParser : IniParserDecorator
{
    #region Fields

    private readonly IOptions<CommentIniParserSettings> _options;
    
    #endregion
  

    public CommentIniParser(
        FileIniDataParser parser,
        IOptions<CommentIniParserSettings>? options = null) : base(parser)
    {
        _options = options ?? Options.Create(CommentIniParserSettings.Default);
        
        if (_options.Value.Comments == null)
        {
            _options = Options.Create(CommentIniParserSettings.Default);
        }
        
        
    }

    public new IniData ReadFile(string filePath)
    {
        using var streamReader = new StreamReader(filePath);
        
        return ReadData(streamReader);
        
    }
    
    public new IniData ReadData(StreamReader streamReader)
    {
        var cleanedContent = GetCleanedContentAsync(streamReader);
        
        using var stream = new MemoryStream(
            Encoding.UTF8.GetBytes(cleanedContent.Result));
        
        using var reader = new StreamReader(stream);
        
        return base.ReadData(reader);
    }

    private async Task<string> GetCleanedContentAsync(StreamReader reader)
    {
        var sb = new StringBuilder();
        
        bool inMultilineComment = false;
        string multilineEnd = null;
        
        while (await reader.ReadLineAsync() is { } line)
        {
            var processedLine = ProcessLine(line, ref inMultilineComment, ref multilineEnd);
            
            if (!string.IsNullOrWhiteSpace(processedLine))
            {
                sb.AppendLine(FormatLine(processedLine));
            }
        }
        
        return sb.ToString();
    }

    private string ProcessLine(string line, ref bool inMultilineComment, ref string multilineEnd)
    {
        
        if (inMultilineComment)
        {
            var endIndex = line.IndexOf(multilineEnd, StringComparison.Ordinal);
            if (endIndex == -1) return null;
            
            inMultilineComment = false;
            return line[(endIndex + multilineEnd.Length)..].Trim();
        }

        var comments = _options.Value.Comments;
        
        if (comments == null)
        {
            throw new Exception("No comments found in settings configuration.");
        }
        
        
        foreach (var comment in comments)
        {
            var startIndex = line.IndexOf(comment.Start, StringComparison.Ordinal);
            
            if (startIndex == -1) continue;
            
            if (comment.Start == "#" && startIndex > 0)
            {
                var beforeComment = line[..startIndex];
                if (beforeComment.Contains("=") || !char.IsWhiteSpace(line[startIndex - 1]))
                    continue;
            }
            
            if (string.IsNullOrEmpty(comment.End))
            {
                return line[..startIndex].Trim();
            }
            
            var endIndex = line.IndexOf(
                comment.End, 
                startIndex + comment.Start.Length, StringComparison.Ordinal);
            
            if (endIndex != -1)
            {
                return (line[..startIndex] + line[(endIndex + comment.End.Length)..]).Trim();
            }

            inMultilineComment = true;
            multilineEnd = comment.End;
            return line[..startIndex].Trim();
            
        }

        return line.Trim();
    }
    
    
    private static string FormatLine(string line) =>
       MyRegex().Replace(line, "$1 = $2");
    

    [GeneratedRegex(@"^(\w+)\s+(\S+)$")]
    private static partial Regex MyRegex();
   
}
