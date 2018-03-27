using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    /// <summary>
    /// Set
    /// </summary>
    /// <typeparam name="T"></typeparam>    
    public class Set<T> : ISet<T> where T: IEquatable<T>
    {
        public int Count => _count;

        public bool IsReadOnly => throw new NotImplementedException();

        public bool Add(T item)
        {
            var notContains = !Contains(item);
            
            if (notContains)
                _items[_count++] = item;

            return notContains;
        }

        public void Clear()
        {
            _count = 0;
        }

        public bool Contains(T item)
        {
            return 
                _count == 0 ? 
                    false
                    //: _items.Take(_count).Any(i => i.Equals(item));
                    : _items.Take(_count).Any(i => i.Equals(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.Take(_count).GetEnumerator();            
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return other.Any(item => Contains(item));
        }

        public bool Remove(T item)
        {       
            var index = Array.FindIndex(_items, i => i.Equals(item));
            if (index >= 0)
            {
                _items = RemoveAt(_items, index);
                _count--;
                return true;
            }

            return false;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (var item in other)
                Add(item);
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private T[] RemoveAt(T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        private T[] _items = new T[100];

        private int _count = 0;
    }
}
