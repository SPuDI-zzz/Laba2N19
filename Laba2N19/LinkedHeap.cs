using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2N19
{
    public class LinkedHeap<T> : IHeap<T> where T : IComparable<T>
    {
        private T _value;

        private int _level;

        private LinkedHeap<T> _parent, _leftChild, _rightChild;

        public int Count { get; private set; }

        public bool IsEmpty { get { return this == null || Count == 0; } private set { } }

        public IEnumerable<T> Nodes { get; private set; }

        public LinkedHeap()
        {
            _value = default;
            Count = 0;
            _level = 0;
            _parent = _leftChild = _rightChild = null;
        }

        public LinkedHeap(T value)
        {
            _value = value;
            Count = 1;
            _level = 2;
            _parent = _leftChild = _rightChild = null;
        }

        public LinkedHeap(T value, LinkedHeap<T> parent)
        {
            _value = value;
            Count = 1;
            _level = 2;
            _parent = parent;
            _leftChild = _rightChild = null;
        }

        public void Add(T value)
        {
            if (IsEmpty)
            {
                _value = value;
                Count++;
                _level = 2;
                return;
            }

            Count++;
            _level = UpLevelAsNeeded();

            if (_leftChild == null)
            {
                SwapAsNeeded(ref value, ref _value);              
                _leftChild = new LinkedHeap<T>(value, this);
                return;
            }
            
            if (_rightChild == null)
            {
                SwapAsNeeded(ref value, ref _value);
                _rightChild = new LinkedHeap<T>(value, this);
                return;
            }
            
            if (IsLeftSide())
            {
                _leftChild.Add(value);
                SwapAsNeeded(ref _leftChild._value, ref _value);
                return;
            }

            _rightChild.Add(value);
            SwapAsNeeded(ref _rightChild._value, ref _value);           
        }

        public void Remove(T value)
        {
            if (IsEmpty)
            {
                throw new HeapException("Cannot remove element from empty Min-Heap");
            }

            LinkedHeap<T> node = SearchRoot(value);
            if (node == null)
            {
                //throw new HeapException("Not found element in Min-Heap");
                return;
            }

            LinkedHeap<T> nodeLastLeafParent = SearchLastLeafParent();
            nodeLastLeafParent.BalanceLevel();

            if (nodeLastLeafParent._rightChild != null)
            {
                if (node.Equals(nodeLastLeafParent._rightChild))
                {
                    nodeLastLeafParent._rightChild = null;
                    return;
                }

                node._value = nodeLastLeafParent._rightChild._value;
                nodeLastLeafParent._rightChild = null;
            }
            else
            {
                if (node.Equals(nodeLastLeafParent._leftChild))
                {
                    nodeLastLeafParent._leftChild = null;
                    return;
                }

                node._value = nodeLastLeafParent._leftChild._value;
                nodeLastLeafParent._leftChild = null;
            }

            node.Heapify();
        }

        public void Clear()
        {
            Count = 0;
            _level = 0;
            _value = default;
            _parent = null;
            _rightChild = null;
            _leftChild = null;
        }

        public bool Contains(T node)
        {
            if (this == null || node == null) return false;

            if (_value.CompareTo(node) == 0) return true;

            if (_value.CompareTo(node) > 0) return _rightChild.Contains(node);

            return _leftChild.Contains(node);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        public void Print()
        {
            if (Count > 0) PrintPretty("", true);
        }

        private void SwapAsNeeded(ref T newValue, ref T oldValue)
        {
            if (newValue.CompareTo(oldValue) == -1)
            {
                (newValue, oldValue) = (oldValue, newValue);
            }
        }

        private bool IsLeftSide()
        {
            return Count < _level - _level / 4;
        }

        private int UpLevelAsNeeded()
        {
            return _level <= Count ? _level * 2 : _level;
        }

        private T this[int index]
        {
            get
            {
                if (index == 0) return _value;

                int level = 2;

                while (level <= index + 1)
                {
                    level *= 2;
                }

                if (index + 1 < level - level / 4)
                {
                    return _leftChild[index / 2];
                }

                return _rightChild[index / 2 - 1];
            }
        }

        private LinkedHeap<T> SearchRoot(T value)
        {
            if (this == null || value == null) return null;

            if (_value.CompareTo(value) == 0) return this;

            if (_value.CompareTo(value) > 0) return null;

            return _leftChild?.SearchRoot(value) ?? _rightChild?.SearchRoot(value);
        }

        private LinkedHeap<T> SearchLastLeafParent()
        {
            if (_leftChild == null && _rightChild == null) return _parent;

            if (IsLeftSide())
            {
                return _leftChild.SearchLastLeafParent();
            }

            return _rightChild.SearchLastLeafParent();
        }

        private void BalanceLevel()
        {
            if (_level % Count == 0) _level /= 2;

            Count--;
            _parent?.BalanceLevel();
        }        

        private void Heapify()
        {
            if (this == null || _leftChild == null) return;
            LinkedHeap<T> child = _leftChild;

            if (_rightChild?._value.CompareTo(_leftChild._value) == -1)
            {
                child = _rightChild;
            }

            if (_value.CompareTo(child._value) == -1) return;
            (child._value, _value) = (_value, child._value);
            child.Heapify();
        }
        
        private void PrintPretty(string indent, bool last)
        {
            Console.Write(indent);

            if (last)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "| ";
            }
            Console.WriteLine(_value);

            var children = new List<LinkedHeap<T>>();

            if (this._leftChild != null)
                children.Add(this._leftChild);
            if (this._rightChild != null)
                children.Add(this._rightChild);

            for (int i = 0; i < children.Count; i++)
                children[i].PrintPretty(indent, i == children.Count - 1);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
