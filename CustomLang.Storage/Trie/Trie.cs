using System.Collections;

namespace CustomLang.Storage.Trie
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Trie<TValue> : ITrie<TValue>, ICloneable
    {
        
        #region Fields

        private readonly HashSet<char> _alphabet;
        private readonly bool[] _allowedChars;

        #endregion
        
        #region Constructors
        
        public Trie(string alphabet)
        {
            if (string.IsNullOrEmpty(alphabet))
                throw new ArgumentException("Alphabet cannot be null or empty.", nameof(alphabet));

            _alphabet = new HashSet<char>(alphabet);
            
            if (_alphabet.Count != alphabet.Length)
                throw new ArgumentException("Alphabet must contain unique characters.", nameof(alphabet));
            
            _allowedChars = new bool[char.MaxValue + 1];
            foreach (var c in _alphabet)
                _allowedChars[c] = true;

            Root = new TrieNode<TValue>(_alphabet.Count);
        }

        private Trie(HashSet<char> alphabet, bool[] allowedChars, TrieNode<TValue> root)
        {
            _alphabet = new HashSet<char>(alphabet);
            _allowedChars = (bool[])allowedChars.Clone();
            Root = (TrieNode<TValue>)root.Clone();
        }
        
        #endregion
        
        #region Properties
        
        public TrieNode<TValue> Root { get; }

        public string Alphabet => _alphabet.ToString()!;
        
        public int Count => _alphabet.Count;
        
        #endregion
        
        #region ICloneable Implementation
        
        public object Clone()
        {
            return new Trie<TValue>(_alphabet, _allowedChars, Root);
        }
        
        #endregion
        
        #region Methods
        
        public bool ContainsKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            return TryGetNode(key, out var node) && node.Value != null;
        }
        
        public bool TryGetValue(string key, out TValue value)
        {
            if (!TryGetNode(key, out var node))
            {
                value = default;
                return false;
            }

            value = node.Value;
            return true;
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (!TryGetNode(key, out var node) || node.Value == null)
                return false;
            
            node.Value = default;
            
            RemoveEmptyNodes(key);
            
            return true;
            
        }

        
        
        
        public void Reset()
        {
            Root.Children.Clear();
        }


        public TValue Obtain(string key)
        {
            return TryGetValue(key, out var value)
                ? value 
                : throw new KeyNotFoundException("Key not found.");
        }

        public TValue this[string key]
        {
            get => Obtain(key);
            set => Add(key, value);
        }

        public void Add(string key, TValue value)
        {
            ValidateKey(key);
            var node = GetOrCreateNode(key);
            node.AddValue(value); 
        }
        
        private bool TryGetNode(string key, out TrieNode<TValue> node)
        {
            node = Root;
            foreach (var k in key)
            {
                if (!_allowedChars[k] || !node.Children.TryGetValue(k, out node))
                {
                    node = null;
                    return false;
                }
            }
            return true;
        }

        
        private int GetCount()
        {
            int count = 0;
            var stack = new Stack<TrieNode<TValue>>();
            stack.Push(Root);

            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (node.Value != null)
                {
                    count++;
                }

                foreach (var child in node.Children.Values)
                {
                    stack.Push(child);
                }
            }

            return count;
        }
        
        private TrieNode<TValue> GetOrCreateNode(string key)
        {
            var node = Root;
            foreach (var k in key)
            {
                var children = node.Children;
                if (!children.TryGetValue(k, out var child))
                {
                    child = new TrieNode<TValue?>(_alphabet.Count);
                    children[k] = child;
                }
                node = child;
            }
            return node;
        }

        private Stack<TrieNode<TValue>> SearchNode(string key)
        {
        
            var node = Root;
            var path = new Stack<TrieNode<TValue>>(key.Length);

            foreach (var k in key)
            {
                if (!_allowedChars[k])
                    throw new ArgumentException($"Invalid character '{k}' in key");

                if (!node.Children.TryGetValue(k, out node))
                    return new Stack<TrieNode<TValue>>();

                path.Push(node);
            }

            return path;
            
        }

        private void ValidateKey(string key)
        {
            foreach (var c in key.Where(c => !_allowedChars[c]))
            {
                throw new ArgumentException($"Invalid character '{c}' in key");
            }
        }

        private void RemoveEmptyNodes(string key)
        {
            var current = Root;
            var path = new Stack<(TrieNode<TValue> Node, char KeyChar)>();
            
            foreach (var c in key)
            {
                if (!_allowedChars[c] || !current.Children.TryGetValue(c, out var next))
                    return;

                path.Push((current, c));
                current = next;
            }
            
            while (path.Count > 0 && current.Children.Count == 0 && current.Value == null)
            {
                var (parent, keyChar) = path.Pop();
                parent.Children.Remove(keyChar);
                current = parent;
            }
        }
        
        #endregion
        
        #region IEnumerable<TValue> Implementation

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            var stack = new Stack<(TrieNode<TValue> Node, string Key)>();
            stack.Push((Root, ""));

            while (stack.Count > 0)
            {
                var (node, currentKey) = stack.Pop();

                if (node.Value != null)
                {
                    yield return new KeyValuePair<string, TValue>(currentKey, node.Value);
                }

                var children = node.Children;
                
                foreach (var (keyChar, childNode) in children)
                {
                    stack.Push((childNode, currentKey + keyChar));
                }
                
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        #endregion
        
    }
}