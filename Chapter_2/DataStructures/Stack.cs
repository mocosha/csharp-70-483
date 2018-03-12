using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack
{
    /// <summary>
    /// Educational purpose only stack implementation
    /// Interface https://msdn.microsoft.com/en-us/library/bb339909(v=vs.110).aspx?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Stack<T>
    {
        public Stack() : this(100) { }

        public Stack(int capacity)
        {
            _items = new T[capacity];
        }

        public void Push(T item)
        {
            if (_currentIndex >= _items.Length)
            { throw new StackOverflowException(); }

            _items[_currentIndex++] = item;
        }

        public T Pop()
        {
            if (_currentIndex <= 0)
            { throw new Exception("Stack is empty"); }

            return _items[--_currentIndex];
        }

        public int Length { get { return _currentIndex; } }

        private T[] _items = null;

        private int _currentIndex = 0;
    }
}
