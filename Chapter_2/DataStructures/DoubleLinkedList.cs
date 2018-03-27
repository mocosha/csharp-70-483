using System;

namespace DataStructures
{
    public class Node<T> where T : IEquatable<T>
    {
        public Node(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _value = value;
        }

        public Node<T> Previous { get; set; }
        public Node<T> Next { get; set; }

        public override string ToString()
        {
            return _value.ToString();
        }

        private T _value;
    }

    public class DoubleLinkedList
    {
        public DoubleLinkedList()
        {
            _head = null;
        }

        public bool IsEmpty()
        {
            return _head == null;
        }

        public string Values()
        {
            if (IsEmpty())
                return "";

            return GetValue(_head);
        }

        public Node<string> Find(string title)
        {
            return Find(_head, title);
        }

        public void Add(string value)
        {
            var nodeForAdd = new Node<string>(value);

            var node = Find(value);
            if (node != null)
                return;

            if (_head == null)
            {
                _head = nodeForAdd;
                _current = _head;
            }
            else
            {
                _current.Next = nodeForAdd;
                nodeForAdd.Previous = _current;
                _current = nodeForAdd;
            }
        }

        public bool Delete(string title)
        {
            var nodeForDelete = Find(title);
            if (nodeForDelete == null)
                return false;
            else
            {
                var next = nodeForDelete.Next;
                var previous = nodeForDelete.Previous;

                if (previous != null)
                    previous.Next = next;

                if (next != null)
                    next.Previous = previous;

                return true;
            }
        }

        private Node<string> Find(Node<string> node, string title)
        {
            if (node == null)
                return null;

            if (string.Equals(node.ToString(), title, StringComparison.OrdinalIgnoreCase))
                return node;

            return Find(node.Next, title);
        }

        private string GetValue(Node<string> node)
        {
            if (node == null)
                return "";

            return $"{node}|" + GetValue(node.Next);
        }

        private Node<string> _head;
        private Node<string> _current;
    }
}
