using System.Collections;
using CustomLang.Interpreter.interfaces;
using CustomLang.Storage;
using CustomLang.Storage.Trie;

namespace CustomLang.Interpreter;

/// <summary>
/// Trie-based implementation of <see cref="IInterpreterStorage{TValue}"/> for efficient string key storage and retrieval
/// </summary>
/// <typeparam name="T">The type of values stored in the trie</typeparam>
/// <remarks>
/// This implementation provides O(n) time complexity for operations where m is the length of the key,
/// making it particularly efficient for storing and retrieving values with string keys.
/// </remarks>
public class TrieStorage<T> : IInterpreterStorage<T>
{
    private readonly ITrie<T> _trie;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrieStorage{T}"/> class
    /// </summary>
    /// <param name="trie">The trie implementation to use as backing storage</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="trie"/> is null</exception>
    public TrieStorage(ITrie<T> trie)
    {
        _trie = trie ?? throw new ArgumentNullException(nameof(trie));
    }

    /// <summary>
    /// Adds or updates a value in the trie with the specified key
    /// </summary>
    /// <param name="key">The key to add or update</param>
    /// <param name="value">The value to store</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null</exception>
    public void SetValue(string key, T value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
       
        _trie.Add(key, value);
    }

    /// <summary>
    /// Attempts to get the value associated with the specified key
    /// </summary>
    /// <param name="key">The key to search for</param>
    /// <param name="value">When found, contains the value associated with the specified key</param>
    /// <returns>true if the key was found; otherwise, false</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null</exception>
    public bool TryGetValue(string key, out T value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        return _trie.TryGetValue(key, out value);
    }

    /// <summary>
    /// Determines whether the trie contains the specified key
    /// </summary>
    /// <param name="key">The key to locate</param>
    /// <returns>true if the trie contains the key; otherwise, false</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null</exception>
    public bool Contains(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        return _trie.ContainsKey(key);
    }

    /// <summary>
    /// Removes the value with the specified key from the trie
    /// </summary>
    /// <param name="key">The key to remove</param>
    /// <returns>true if the element was successfully removed; otherwise, false</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null</exception>
    public bool Remove(string key)
    {
        return _trie.Remove(key);
    }

    /// <summary>
    /// Gets the number of key/value pairs contained in the trie
    /// </summary>
    public int Count => _trie.Count();

    /// <summary>
    /// Gets or sets the value associated with the specified key
    /// </summary>
    /// <param name="key">The key of the value to get or set</param>
    /// <returns>The value associated with the specified key</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null</exception>
    /// <exception cref="KeyNotFoundException">Thrown when getting a key that doesn't exist</exception>
    public T this[string key]
    {
        get => _trie[key];
        set => _trie[key] = value;
    }

    /// <summary>
    /// Gets a collection containing all keys in the trie
    /// </summary>
    public IEnumerable<string> Keys => _trie.Select(kvp => kvp.Key);

    /// <summary>
    /// Gets a collection containing all values in the trie
    /// </summary>
    public IEnumerable<T> Values => _trie.Select(kvp => kvp.Value);

    /// <summary>
    /// Creates a new trie that is a deep copy of the current instance
    /// </summary>
    /// <returns>A new trie containing all the key/value pairs of the current instance</returns>
    public IInterpreterStorage<T> Clone()
    {
        return new TrieStorage<T>((Trie<T>)_trie.Clone());
    }

    /// <summary>
    /// Removes all keys and values from the trie
    /// </summary>
    public void Clear()
    {
        _trie.Reset();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the key/value pairs in the trie
    /// </summary>
    /// <returns>An enumerator for the trie</returns>
    public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
    {
        return _trie.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the key/value pairs in the trie
    /// </summary>
    /// <returns>An enumerator for the trie</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}