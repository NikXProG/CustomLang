namespace CustomLang.Storage.Trie
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class TrieNode<TValue> : ICloneable
    {
        
        #region Fields

        private readonly Dictionary<char, TrieNode<TValue>> _children;
        private TValue _value;

        #endregion
        
        #region Constructors
        public TrieNode(int alphabetSize)
        {
            _children = new Dictionary<char, TrieNode<TValue>>(alphabetSize);
        }
        
        private TrieNode(Dictionary<char, TrieNode<TValue>> children, TValue value)
        {
            _children = new Dictionary<char, TrieNode<TValue>>(children);
            _value = value;
        }
        
        #endregion
        
        #region Properties

        public TValue Value { get=>_value; set => _value = value; }

        public Dictionary<char, TrieNode<TValue>> Children => _children;
        
        #endregion
        
        #region ICloneable Implementation
        public object Clone()
        {
            
            var clonedChildren = new Dictionary<char, TrieNode<TValue>>(_children.Count);
            foreach (var kvp in _children)
            {
                clonedChildren.Add(kvp.Key, (TrieNode<TValue>)kvp.Value.Clone());
            }
            
            TValue clonedValue = _value;
            if (_value is ICloneable cloneableValue)
            {
                clonedValue = (TValue)cloneableValue.Clone();
            }

            return new TrieNode<TValue>(clonedChildren, clonedValue);
        }
        #endregion
        
        #region Methods
        public void AddValue(TValue value)
        {
            _value = value;
        }
        
        public bool TryRemoveChild(char key)
        {
            return _children.Remove(key);
        }
        
        #endregion
    }    
}
