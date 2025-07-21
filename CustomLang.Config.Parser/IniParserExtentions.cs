using System.Reflection;
using CustomLang.Config.Parser.Decorators.IniParserDecorator;
using IniParser;
using IniParser.Model;

namespace CustomLang.Config.Parser;

public static class IniParserExtentions
{

    public static bool TryGetValue(this IniData data, string sectionName, string key,  out string value)
    {
        value = default;
        
        if (string.IsNullOrWhiteSpace(sectionName) || string.IsNullOrWhiteSpace(key))
            return false;

        var section = data[sectionName];
        if (section == null)
            return false;
        
        value = section[key];
        return value != null;

    }
    public static T LoadFromIni<T>(this CommentIniParser parser, string filePath) where T : new()
    {
        var config = new T();
        var iniData = parser.ReadFile(filePath);
        
        foreach (var prop in typeof(T).GetProperties())
        {
            var attr = prop.GetCustomAttribute<IniPropertyAttribute>();
            if (attr == null) continue;

            if (iniData.TryGetValue(attr.Section, attr.Key, out var value ))
            {
                prop.SetValue(config, ConvertValue(value, prop.PropertyType));
            }
            else if (!attr.Optional)
            {
                throw new Exception(
                    $"Required INI property not found: [{attr.Section}][{attr.Key}]");
                
            }
        }

        return config;
    }
    
    private static object ConvertValue(string value, Type targetType)
    {
        if (targetType == typeof(string))
            return value;
    
        return targetType.IsEnum ?
            Enum.Parse(targetType, value, true) :
            Convert.ChangeType(value, targetType);
    }


}