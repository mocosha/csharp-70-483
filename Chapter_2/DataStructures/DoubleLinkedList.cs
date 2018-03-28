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
                return "Empty list";

            return GetValue(_head);
        }

        public Node<string> Find(string title)
        {
            return Find(_head, title);
        }

        private Node<string> GetLast(Node<string> node)
        {
            if (node == null)
                return null;

            if (node.Next == null)
                return node;

            return GetLast(node.Next);
        }

        public void AddToEnd(string value)
        {
            var nodeForAdd = new Node<string>(value);

            var node = Find(value);
            if (node != null)
                return;

            if (_head == null)
            {
                _head = nodeForAdd;
            }
            else
            {
                var last = GetLast(_head);

                nodeForAdd.Previous = last;
                last.Next = nodeForAdd;
            }
        }

        public bool Delete(string title)
        {
            if (IsEmpty())
                return false;

            var nodeForDelete = Find(title);
            if (nodeForDelete == null)
                return false;
            else
            {
                var next = nodeForDelete.Next;
                var previous = nodeForDelete.Previous;

                if (next == null && previous == null)
                {
                    _head = null;
                    return true;
                }


                if (previous == null) // delete head
                {
                    var newHead = _head.Next;
                    newHead.Previous = null;
                    _head.Next = null;
                    _head = newHead;
                    return true;
                }

                if (next == null) // delete last
                {
                    var newLast = nodeForDelete.Previous;
                    newLast.Next = null;
                    nodeForDelete.Previous = null;
                    return true;
                }

                previous.Next = next;
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
    }
}
