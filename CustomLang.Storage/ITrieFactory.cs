namespace CustomLang.Storage;

public interface ITrieFactory
{
    public ITrie<T> Create<T>();
}