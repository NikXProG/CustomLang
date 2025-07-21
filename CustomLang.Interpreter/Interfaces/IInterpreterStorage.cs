using System.Collections;

namespace CustomLang.Interpreter.interfaces;

public interface IInterpreterStorage<TValue> : IEnumerable<KeyValuePair<string, TValue>> 
{
    /// <summary>
    /// Adds or updates a value by the specified key
    /// </summary>
    void SetValue(string key, TValue value);

    /// <summary>
    /// Tries to get a value by key
    /// </summary>
    bool TryGetValue(string key, out TValue value);
    
    //clone
    IInterpreterStorage<TValue> Clone();

    /// <summary>
    /// Checks if the key is in the storage
    /// </summary>
    bool Contains(string key);

    /// <summary>
    /// Removes a value by key
    /// </summary>
    bool Remove(string key);

    /// <summary>
    /// Clears the storage
    /// </summary>
    void Clear();

    /// <summary>
    /// Returns the number of elements in the storage
    /// </summary>
    int Count
    {
        get;
    }

    /// <summary>
    /// Gets or sets the value by key
    /// </summary>
    TValue this[string key]
    {
        get;
        set;
    }

    /// <summary>
    /// Returns all keys in the store
    /// </summary>
    IEnumerable<string> Keys
    {
        get;
    }

    /// <summary>
    /// Returns all values in the store
    /// </summary>
    IEnumerable<TValue> Values
    {
        get;
    }
    
}
