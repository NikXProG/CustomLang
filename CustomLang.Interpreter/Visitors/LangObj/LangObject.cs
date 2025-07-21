namespace CustomLang.Interpreter.Visitors.LangObj;

public class LangObject 
{
    public LangObject()
    {
        
    }
    
    public virtual LangType GetObjType()
    {
        return LangType.Object;
    }
    
}