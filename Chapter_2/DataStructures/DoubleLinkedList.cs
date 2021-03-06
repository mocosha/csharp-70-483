﻿using System;
using System.Collections;
using System.Collections.Generic;

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

        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }

        public T Value => _value;
        private T _value;

        public override string ToString()
        {
            return _value.ToString();
        }
    }

    public class DoubleLinkedList<T>: IEnumerable<T> where T : IEquatable<T>
    {
        public DoubleLinkedList()
        {
            _head = null;
        }

        public bool IsEmpty()
        {
            return _head == null;
        }

        public Node<T> Find(T value)
        {
            return Find(_head, value);
        }

        private Node<T> GetLast(Node<T> node)
        {
            if (node == null)
                return null;

            if (node.Right == null)
                return node;

            return GetLast(node.Right);
        }

        public void AddToEnd(T value)
        {
            var nodeForAdd = new Node<T>(value);

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

                nodeForAdd.Left = last;
                last.Right = nodeForAdd;
            }
        }

        public bool Delete(T title)
        {
            if (IsEmpty())
                return false;

            var nodeForDelete = Find(title);
            if (nodeForDelete == null)
                return false;
            else
            {
                var next = nodeForDelete.Right;
                var previous = nodeForDelete.Left;

                if (next == null && previous == null)
                {
                    _head = null;
                    return true;
                }

                if (previous == null) // delete head
                {
                    var newHead = _head.Right;
                    newHead.Left = null;
                    _head.Right = null;
                    _head = newHead;
                    return true;
                }

                if (next == null) // delete last
                {
                    var newLast = nodeForDelete.Left;
                    newLast.Right = null;
                    nodeForDelete.Left = null;
                    return true;
                }

                previous.Right = next;
                next.Left = previous;
                return true;
            }
        }

        private Node<T> Find(Node<T> node, T value)
        {
            if (node == null)
                return null;

            if (Equals(node.Value, value))
                return node;

            return Find(node.Right, value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = _head;
            while (node != null)
            {
                yield return node.Value;
                node = node.Right;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Node<T> _head;
    }
}
