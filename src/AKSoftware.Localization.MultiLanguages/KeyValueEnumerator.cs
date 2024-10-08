using System;
using System.Collections;
using System.Collections.Generic;

namespace AKSoftware.Localization.MultiLanguages
{
    public class KeyValueEnumerator : IEnumerator<KeyValuePair<object, object>>
    {
        private readonly Stack<IEnumerator<KeyValuePair<object, object>>> _stack = new Stack<IEnumerator<KeyValuePair<object, object>>>();
        private KeyValuePair<object, object> _current;
        private string _currentParentKey = string.Empty;

        public KeyValueEnumerator(IReadOnlyDictionary<object, object> dictionary)
        {
            // Initialize with the top-level dictionary enumerator
            _stack.Push(dictionary.GetEnumerator());
        }

        public object Current => _current;

        object IEnumerator.Current => Current;

        KeyValuePair<object, object> IEnumerator<KeyValuePair<object, object>>.Current => _current;

        public bool MoveNext()
        {
            // Continue iterating while there are items in the stack
            while (_stack.Count > 0)
            {
                var enumerator = _stack.Peek();

                // If we reach the end of the current enumerator, pop it off the stack
                if (!enumerator.MoveNext())
                {
                    _stack.Pop();
                    _currentParentKey = RemoveLastKeyFromCurrentParentKey(_currentParentKey);
                    continue;
                }

                var kvp = enumerator.Current;

                // If the value is a string, set it as the current item and return true
                if (kvp.Value is string)
                {
                    _current = new KeyValuePair<object, object>($"{_currentParentKey}{kvp.Key}", kvp.Value);
                    return true;
                }

                // If the value is another dictionary, push its enumerator onto the stack
                if (kvp.Value is IReadOnlyDictionary<object, object> nestedDictionary)
                {
                    _currentParentKey = $"{_currentParentKey}{kvp.Key}:";
                    _stack.Push(nestedDictionary.GetEnumerator());
                }
            }

            // No more items left to enumerate
            return false;
        }

        private string RemoveLastKeyFromCurrentParentKey(string key)
        {
            var lastColon = key.LastIndexOf(':');
            if (lastColon > 0) // Has a parent 
            {
                key = _currentParentKey.Substring(0, _currentParentKey.Length - 1);
                lastColon = key.LastIndexOf(':');
                if (lastColon > 0)
                    return key.Substring(0, lastColon + 1);
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        public void Reset()
        {
            throw new NotSupportedException("Resetting the enumerator is not supported.");
        }

        public void Dispose()
        {
            while (_stack.Count > 0)
            {
                _stack.Pop().Dispose();
            }
        }
    }
}

