using System;

namespace DataStructures
{
    public class Node
    {
        public Node(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            _value = value;
        }

        public Node Previous { get; set; }
        public Node Next { get; set; }

        public override string ToString()
        {
            return _value;
        }

        private string _value;
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

        public Node Find(string title)
        {
            return Find(_head, title);
        }

        public void Add(string value)
        {
            var nodeForAdd = new Node(value);

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

        private Node Find(Node node, string title)
        {
            if (node == null)
                return null;

            if (string.Equals(node.ToString(), title, StringComparison.OrdinalIgnoreCase))
                return node;

            return Find(node.Next, title);
        }

        private string GetValue(Node node)
        {
            if (node == null)
                return "";

            return $"{node}|" + GetValue(node.Next);
        }

        private Node _head;
        private Node _current;
    }
}
