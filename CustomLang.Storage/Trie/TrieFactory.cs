namespace CustomLang.Storage.Trie;

public class TrieFactory : ITrieFactory
{
    private readonly string _alphabet;
    
    
    public TrieFactory(string alphabet)
    {
        _alphabet = alphabet;
    }
    
    public ITrie<T> Create<T>()
    {
        return new Trie<T>(_alphabet);
    }
}