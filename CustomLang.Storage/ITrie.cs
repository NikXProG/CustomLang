using System.Collections;

namespace CustomLang.Storage;


public interface ITrie<TValue> : IEnumerable<KeyValuePair<string, TValue>>, IEnumerable
{
   
    public  TValue  Obtain(
        string key);

    public bool Remove(
        string key);
    
    public void Add(
        string key, 
        TValue value);
    
    public bool TryGetValue(
        string key, 
        out TValue value);

    public bool ContainsKey(
        string key);

    public object Clone();

    public void Reset();
  
    public TValue this[string key]
    {
        get;
        set;
    }


    public string Alphabet
    {
        get;
    }
    
    
}