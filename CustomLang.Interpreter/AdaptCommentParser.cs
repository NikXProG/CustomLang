using Sprache;
using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.interfaces;

namespace CustomLang.Interpreter;
public class AdaptCommentParser : IComment
  {
    
    #region Constructors
    
    /// <summary>
    /// Initializes a Comment with C-style headers and Windows newlines.
    /// </summary>
    public AdaptCommentParser()
    {
      this.Single = "//";
      this.MultiOpen = "/*";
      this.MultiClose = "*/";
      this.NewLine = "\n";
    }

    /// <summary>
    /// Initializes a Comment with custom multi-line headers and newline characters.
    /// Single-line headers are made null, it is assumed they would not be used.
    /// </summary>
    /// <param name="multiOpen"></param>
    /// <param name="multiClose"></param>
    /// <param name="newLine"></param>
    public AdaptCommentParser(string multiOpen, string multiClose, string newLine)
    {
      this.Single = (string) null;
      this.MultiOpen = multiOpen;
      this.MultiClose = multiClose;
      this.NewLine = newLine;
    }

    /// <summary>
    /// Initializes a Comment with custom headers and newline characters.
    /// </summary>
    /// <param name="single"></param>
    /// <param name="multiOpen"></param>
    /// <param name="multiClose"></param>
    /// <param name="newLine"></param>
    public AdaptCommentParser(string single, string multiOpen, string multiClose, string newLine)
    {
      this.Single = single;
      this.MultiOpen = multiOpen;
      this.MultiClose = multiClose;
      this.NewLine = newLine;
    }
    
    #endregion
    
    #region Properties
    
    #region Base Properties
    
    /// <summary>Single-line comment header.</summary>
    public string Single { get; set; }

    /// <summary>Newline character preference.</summary>
    public string NewLine { get; set; }

    /// <summary>Multi-line comment opener.</summary>
    public string MultiOpen { get; set; }

    /// <summary>Multi-line comment closer.</summary>
    public string MultiClose { get; set; }

    #endregion
    
    /// <summary>Parse a single-line comment.</summary>
    public Parser<string> SingleLineComment
    {
      get
      {
        if (this.Single == null)
          throw new ParseException("Field 'Single' is null; single-line comments not allowed.");
        return Parse
          .String(this.Single)
          .SelectMany<IEnumerable<char>, string, string>(
            (Func<IEnumerable<char>, Parser<string>>) 
            (first => 
              Parse.CharExcept(this.NewLine).Many<char>().Text()),
            (Func<IEnumerable<char>, string, string>) 
            ((first, rest) => Single + rest));
      }
    }

    /// <summary>Parse a multi-line comment.</summary>
    public Parser<string> MultiLineComment
    {
      get
      {
        if (this.MultiOpen == null)
          throw new ParseException("Field 'MultiOpen' is null; multi-line comments not allowed.");
        if (this.MultiClose == null)
          throw new ParseException("Field 'MultiClose' is null; multi-line comments not allowed.");
        return 
          Parse.String(this.MultiOpen)
            .SelectMany<IEnumerable<char>, string, string>(
              (Func<IEnumerable<char>, Parser<string>>) 
              (first => 
                Parse.AnyChar.Until<char, IEnumerable<char>>
                  (Parse.String(this.MultiClose)).Text()),
              (Func<IEnumerable<char>, string, string>)
              ((first, rest) => MultiOpen + rest + MultiClose));
      }
    }

    /// <summary>Parse a comment.</summary>
    public Parser<string> AnyComment
    {
      get
      {
        if (this.Single != null && this.MultiOpen != null && this.MultiClose != null)
          return this.SingleLineComment.Or<string>(this.MultiLineComment);
        if (this.Single != null && (this.MultiOpen == null || this.MultiClose == null))
          return this.SingleLineComment;
        if (this.Single == null && this.MultiOpen != null && this.MultiClose != null)
          return this.MultiLineComment;
        throw new ParseException("Unable to parse comment; check values of fields 'MultiOpen' and 'MultiClose'.");
      }
    }
    
    #endregion
    
}
