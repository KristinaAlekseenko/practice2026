using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace task03
{
    public class CustomCollection<T> : IEnumerable<T>
    {
        private T[] _buffer = Array.Empty<T>();
        private int _size = 0;

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Null elements are not allowed");

            if (_size == _buffer.Length)
            {
                var newBuffer = new T[_size == 0 ? 4 : _size * 2];
                for (int i = 0; i < _size; i++)
                    newBuffer[i] = _buffer[i];
                _buffer = newBuffer;
            }
            _buffer[_size++] = item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _size; i++)
                yield return _buffer[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<T> GetReverseEnumerator()
        {
            for (int i = _size - 1; i >= 0; i--)
                yield return _buffer[i];
        }

        public static IEnumerable<int> GenerateSequence(int start, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative");

            int current = start;
            for (int i = 0; i < count; i++)
            {
                yield return current;
                current++;
            }
        }

        public IEnumerable<T> FilterAndSort(Func<T, bool> filter, Func<T, IComparable> keySelector)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter), "Filter function required");
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector), "Key selector function required");

            var filteredList = new List<T>();
            for (int i = 0; i < _size; i++)
            {
                if (filter(_buffer[i]))
                    filteredList.Add(_buffer[i]);
            }

            for (int i = 0; i < filteredList.Count - 1; i++)
            {
                for (int j = 0; j < filteredList.Count - 1 - i; j++)
                {
                    if (keySelector(filteredList[j]).CompareTo(keySelector(filteredList[j + 1])) > 0)
                    {
                        var temp = filteredList[j];
                        filteredList[j] = filteredList[j + 1];
                        filteredList[j + 1] = temp;
                    }
                }
            }

            return filteredList;
        }
    }
}
