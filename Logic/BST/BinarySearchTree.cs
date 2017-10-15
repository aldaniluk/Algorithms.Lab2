using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class BinarySearchTree<T> : IEnumerable<T>
    {
        #region fields
        private Node<T> root;
        private readonly Comparison<T> comparison;
        #endregion

        #region properties
        public bool IsReadOnly => false;
        #endregion

        #region ctors
        public BinarySearchTree()
        {
            this.comparison = DefaultCompare();
        }

        public BinarySearchTree(Comparison<T> comparison)
        {
            this.comparison = (comparison == null) ? DefaultCompare() : comparison;
        }

        public BinarySearchTree(IComparer<T> comparer) : this(comparer.Compare) { }

        public BinarySearchTree(T value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException($"{nameof(value)} is null.");
            this.comparison = DefaultCompare();
            root = new Node<T>(value, null);
        }

        public BinarySearchTree(T value, Comparison<T> comparison)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException($"{nameof(value)} is null.");
            this.comparison = (comparison == null) ? DefaultCompare() : comparison;
            root = new Node<T>(value, null);
        }

        public BinarySearchTree(T value, IComparer<T> comparer) : this(value, comparer.Compare) { }

        public BinarySearchTree(T[] values)
        {
            if (values == null) throw new ArgumentNullException($"{nameof(values)} is null.");
            if (values.Length == 0) throw new ArgumentException($"{nameof(values)} is empty.");
            this.comparison = DefaultCompare();
            foreach (var v in values)
            {
                Add(v);
            }
        }

        public BinarySearchTree(T[] values, Comparison<T> comparison)
        {
            if (values == null) throw new ArgumentNullException($"{nameof(values)} is null.");
            if (values.Length == 0) throw new ArgumentException($"{nameof(values)} is empty.");
            this.comparison = (comparison == null) ? DefaultCompare() : comparison;
            foreach (var v in values)
            {
                Add(v);
            }
        }

        public BinarySearchTree(T[] values, IComparer<T> comparer) : this(values, comparer.Compare) { }
        #endregion

        #region public methods
        #region Add, Clear, Contains, Remove
        public void Add(T item)
        {
            if (ReferenceEquals(item, null)) throw new ArgumentNullException($"{nameof(item)} is null.");
            if (Contains(item)) return;

            Node<T> node = root;
            Node<T> parent = null;

            while (node != null)
            {
                if (comparison.Invoke(item, node.Value) < 0)
                {
                    parent = node;
                    node = node.Left;
                }
                else if (comparison.Invoke(item, node.Value) > 0)
                {
                    parent = node;
                    node = node.Right;
                }
            }
            if (ReferenceEquals(root, null)) root = new Node<T>(item, null);
            else
            {
                if (comparison.Invoke(parent.Value, item) > 0)
                    parent.Left = new Node<T>(item, parent);
                else
                    parent.Right = new Node<T>(item, parent);
            }
        }

        public void Clear()
        {
            root = null;
        }

        public bool Contains(T item)
        {
            if (ReferenceEquals(item, null)) throw new ArgumentNullException($"{nameof(item)} is null.");

            Node<T> node = root;

            while (node != null)
            {
                if (comparison.Invoke(node.Value, item) == 0)
                    return true;
                else if (comparison.Invoke(node.Value, item) > 0)
                    node = node.Left;
                else
                    node = node.Right;
            }

            return false;
        }

        public bool Remove(T item)
        {
            if (ReferenceEquals(item, null)) throw new ArgumentNullException($"{nameof(item)} is null.");
            if (ReferenceEquals(root, null)) return false;
            if (!Contains(item)) return false;

            Node<T> find = Find(item);

            if (find.Left == null && find.Right == null)
            {
                return RemoveRightLeftNull(find);
            }
            else if (find.Right == null) //1
            {
                return RemoveRightNull(find);
            }
            else if (find.Right.Left == null) //2
            {
                return RemoveLeftNull(find);
            }
            else //3
            {
                return RemoveRightLeftNotNull(find);
            }
        }
        #endregion

        #region Traversals
        public IEnumerable<T> PreorderTraversal()
        {
            Node<T> current = root;
            Stack<Node<T>> s = new Stack<Node<T>>();

            while (true)
            {
                while (current != null)
                {
                    yield return current.Value;
                    s.Push(current);
                    current = current.Left;
                }
                if (s.Count == 0) break;
                current = s.Pop();
                current = current.Right;
            }
        }

        public IEnumerable<T> InorderTraversal()
        {
            Node<T> current = root;
            Stack<Node<T>> s = new Stack<Node<T>>();

            while (true)
            {
                if (current != null)
                {
                    s.Push(current);
                    current = current.Left;
                }
                else
                {
                    if (s.Count == 0) break;
                    current = s.Pop();
                    yield return current.Value;
                    current = current.Right;
                }
            }
        }

        public IEnumerable<T> PostorderTraversal()
        {
            Node<T> lastVisited = root;

            Stack<Node<T>> stack = new Stack<Node<T>>();
            stack.Push(lastVisited);

            while (stack.Count != 0)
            {
                Node<T> next = stack.Peek();

                bool finishedSubtreesR = next.Right != null ? (comparison.Invoke(next.Right.Value, lastVisited.Value) == 0) : false;
                bool finishedSubtreesL = next.Left != null ? (comparison.Invoke(next.Left.Value, lastVisited.Value) == 0) : false;
                bool isLeaf = (next.Left == null && next.Right == null);

                if (finishedSubtreesR || finishedSubtreesL || isLeaf)
                {
                    stack.Pop();
                    yield return next.Value;
                    lastVisited = next;
                }
                else
                {
                    if (next.Right != null) stack.Push(next.Right);
                    if (next.Left != null) stack.Push(next.Left);
                }
            }
        }

        public IEnumerator<T> GetEnumerator() => PreorderTraversal().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => PreorderTraversal().GetEnumerator();
        #endregion

        #region Rotations
        public void RotateLeft(T value)
        {
            Node<T> current = root;
            Node<T> x = null;
            int result;

            while (current != null)
            {
                result = comparison.Invoke(current.Value, value);
                if (result == 0)
                {
                    x = current;
                    break;
                }
                else if (result > 0)
                    current = current.Left;
                else
                    current = current.Right;
            }

            Node<T> y = x.Right;
            x.Right = y.Left;

            if (y.Left != null)
            {
                y.Left.Parent = x;
            }

            y.Parent = x.Parent;

            if (x.Parent == null)
            {
                root = y;
            }
            else if (x == x.Parent.Left)
            {
                x.Parent.Left = y;
            }
            else
            {
                x.Parent.Right = y;
            }

            y.Left = x;
            x.Parent = y;
        }

        public void RotateRight(T value)
        {
            Node<T> current = root;
            Node<T> x = null;
            int result;

            while (current != null)
            {
                result = comparison.Invoke(current.Value, value);
                if (result == 0)
                {
                    x = current;
                    break;
                }
                else if (result > 0)
                    current = current.Left;
                else
                    current = current.Right;
            }

            Node<T> y = x.Left;
            x.Left = y.Right;

            if (y.Right != null)
            {
                y.Right.Parent = x;
            }

            y.Parent = x.Parent;

            if (x.Parent == null)
            {
                root = y;
            }
            else if (x == x.Parent.Left)
            {
                x.Parent.Left = y;
            }
            else
            {
                x.Parent.Right = y;
            }

            y.Right = x;
            x.Parent = y;
        }
        #endregion

        #region Print by levels
        public void PrintByLevels()
        {
            int n = 0;
            while (RecursionPrint(root, n))
            {
                Console.WriteLine();
                n++;
            }
        }

        private bool RecursionPrint(Node<T> node, int n)
        {
            if (node == null) return false;

            if (n == 0)
            {
                Console.Write(node.Value + " ");
                return node.Left != null || node.Right != null;
            }
            else
            {
                bool l = RecursionPrint(node.Left, n - 1);
                bool r = RecursionPrint(node.Right, n - 1);
                return l || r;
            }
        }
        #endregion

        #endregion

        #region private methods
        private bool RemoveRightLeftNull(Node<T> find)
        {
            Node<T> findparent = find.Parent;
            if (findparent == null)
            {
                root = null;
                return true;
            }
            if (findparent.Left != null)
            {
                if (comparison.Invoke(find.Value, findparent.Left.Value) == 0)
                    findparent.Left = null;
            }
            if (findparent.Right != null)
            {
                if (comparison.Invoke(find.Value, findparent.Right.Value) == 0)
                    findparent.Right = null;
            }
            return true;
        }

        private bool RemoveRightNull(Node<T> find)
        {
            Node<T> findparent = find.Parent;
            if (findparent == null)
            {
                root = find.Left;
                return true;
            }
            if (comparison.Invoke(find.Value, findparent.Value) < 0)
                findparent.Left = find.Left;
            else
                findparent.Right = find.Left;
            return true;
        }

        private bool RemoveLeftNull(Node<T> find)
        {
            Node<T> findparent = find.Parent;
            find.Right.Left = find.Left;
            if (findparent == null)
            {
                root = find.Right;
                return true;
            }
            find.Right.Left = find.Left;
            if (comparison.Invoke(find.Value, findparent.Value) < 0)
                findparent.Left = find.Right;
            else
                findparent.Right = find.Right;
            return true;
        }

        private bool RemoveRightLeftNotNull(Node<T> find)
        {
            Node<T> findparent = find.Parent;

            Node<T> leftmost = find.Right.Left;
            Node<T> leftmostparent = find.Right;
            while (leftmost.Left != null)
            {
                leftmostparent = leftmost;
                leftmost = leftmost.Left;
            }
            leftmostparent.Left = leftmost.Right;
            leftmost.Left = find.Left;
            leftmost.Right = find.Right;
            if (findparent == null)
            {
                root = leftmost;
                return true;
            }
            if (comparison.Invoke(find.Value, findparent.Value) < 0)
                findparent.Left = leftmost;
            else
                findparent.Right = leftmost;
            return true;
        }

        private Node<T> Find(T item)
        {
            if (ReferenceEquals(item, null)) throw new ArgumentNullException($"{nameof(item)} is null.");

            Node<T> node = root;

            while (node != null)
            {
                if (comparison.Invoke(node.Value, item) == 0)
                    return node;
                else if (comparison.Invoke(node.Value, item) > 0)
                    node = node.Left;
                else
                    node = node.Right;
            }

            return null;
        }

        private Comparison<T> DefaultCompare()
        {
            if (typeof(IComparable).IsAssignableFrom(typeof(T)) || typeof(IComparable<T>).IsAssignableFrom(typeof(T))) //Kate's idea!
                return Comparer<T>.Default.Compare;
            else
                throw new ArgumentException($"In type {typeof(T)} there isn't default comparer!");
        }
        #endregion

        #region Node<T>
        private class Node<T>
        {
            #region properties
            public T Value { get; }
            public Node<T> Left { get; set; }
            public Node<T> Right { get; set; }
            public Node<T> Parent { get; set; }
            #endregion

            #region ctors
            public Node() { }

            public Node(T value, Node<T> parent)
            {
                if (ReferenceEquals(value, null)) throw new ArgumentNullException($"{nameof(value)} is null.");

                Value = value;
                Parent = parent;
            }
            #endregion

            #endregion

        }
    }
}
