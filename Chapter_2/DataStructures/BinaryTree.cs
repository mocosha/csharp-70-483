using System;
using System.Collections.Generic;

namespace DataStructures
{
    public interface IBinaryTree<T> where T : IEquatable<T>
    {
        void InsertNode(T value);
        bool IsEmpty();
        IEnumerable<T> GetOrdered();
        IEnumerable<T> GetOrderedReverse();
        Node<T> FindNode(T value);
        void Delete();
    }

    public class NumberBinaryTree : IBinaryTree<int>
    {
        private Node<int> _root { get; set; }
        private Stack<int> _stack { get; set; }

        public IEnumerable<int> GetOrdered()
        {
            List<int> list = new List<int>();

            AddLeftNodeFirst(_root, list);

            return list;
        }

        public IEnumerable<int> GetOrderedV2()
        {
            return AddLeftNodeFirstV2(_root);
        }
        public IEnumerable<int> GetOrderedReverse()
        {
            List<int> list = new List<int>();

            AddRightNodeFirst(_root, list);

            return list;
        }

        private void AddLeftNodeFirst(Node<int> node, List<int> list)
        {
            if (node.Left != null)
            {
                AddLeftNodeFirst(node.Left, list);
            }

            list.Add(node.Value);

            if (node.Right != null)
            {
                AddLeftNodeFirst(node.Right, list);
            }
        }

        private IEnumerable<int> AddLeftNodeFirstV2(Node<int> node)
        {
            if (node.Left != null)
            {
                foreach (var x in AddLeftNodeFirstV2(node.Left))
                    yield return x;
            }

            yield return node.Value;

            if (node.Right != null)
            {
                foreach (var x in AddLeftNodeFirstV2(node.Right))
                    yield return  x;
            }
        }

        private void AddRightNodeFirst(Node<int> node, List<int> list)
        {
            if (node.Right != null)
            {
                AddRightNodeFirst(node.Right, list);
            }

            list.Add(node.Value);

            if (node.Left != null)
            {
                AddRightNodeFirst(node.Left, list);
            }
        }

        public Node<int> FindNode(int value)
        {
            if (_root == null)
            {
                return null;
            }

            return FindNode(_root, value);
        }

        private Node<int> FindNode(Node<int> current, int value)
        {
            if (value == current.Value || current == null)
            {
                return current;
            }

            if (value > current.Value)
            {
                return FindNode(current.Right, value);
            }
            else
            {
                return FindNode(current.Left, value);
            }
        }

        public void InsertNode(int value)
        {
            if (_root == null)
            {
                _root = new Node<int>(value);
            }
            else
            {
                InsertNode(_root, value);
            }
        }

        private void InsertNode(Node<int> current, int value)
        {
            if (value > current.Value)
            {
                if (current.Right == null)
                {
                    current.Right = new Node<int>(value);
                    PrintNode(current.Right);
                }
                else
                {
                    InsertNode(current.Right, value);
                }
            }
            else
            {
                if (current.Left == null)
                {
                    current.Left = new Node<int>(value);
                    PrintNode(current.Left);
                }
                else
                {
                    InsertNode(current.Left, value);
                }
            }
        }

        public bool IsEmpty()
        {
            return _root == null;
        }

        public void Delete()
        {
            _root = null;
        }

        private void PrintNode(Node<int> node)
        {
            if (node != null)
            {
                Console.WriteLine($"Node: value={node.Value}; hash={node.GetHashCode()}");
            }
        }
    }
}
