namespace CustomLang.Interpreter.Visitors.LangObj;

public class String : LangObject
{
    private string _value;
    public String(string value) 
    {
        _value = value;
    }

    public override LangType GetObjType()
    {
        return LangType.String;
    }

    public String Add(LangObject other)
    {
        return new String(_value + other);
    }
    
    public static String operator +(String a, LangObject b) => a.Add(b);
    
    
    public override string ToString() => _value;
    
}