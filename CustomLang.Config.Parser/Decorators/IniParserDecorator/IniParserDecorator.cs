using IniParser;
using IniParser.Model;

namespace CustomLang.Config.Parser.Decorators.IniParserDecorator;

public abstract class IniParserDecorator
{
    private readonly FileIniDataParser _parser;
    
    protected IniParserDecorator(FileIniDataParser parser)
    {
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    }
    
    public virtual IniData ReadFile(string filePath) => _parser.ReadFile(filePath);
    
    public virtual IniData ReadData(StreamReader streamReader) => _parser.ReadData(streamReader);
    
}

