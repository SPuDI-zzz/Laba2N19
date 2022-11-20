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
            return _level <= Count ? _level * 2 : _level ;
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

        /*public T this[int index]
        {
            get
            {
                if (index == 0) return _value;
                if (index) return -_leftChild
                        _rightChild
            }
        }*/

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

        private void Heapify()
        {
            if (this == null) return;
            LinkedHeap<T> child = _leftChild;

            if (_rightChild?._value.CompareTo(_leftChild._value) == -1)
            {
                child = _rightChild;
            }

            if (_value.CompareTo(child._value) == -1) return;
            (child._value, _value) = (_value, child._value);
            child.Heapify();
        }

        private void Heapify(LinkedHeap<T> root) 
        {
            if (root == null) return;
            LinkedHeap<T> child = root._leftChild;

            if (root._rightChild?._value.CompareTo(root._leftChild._value) == -1)
            {
                child = root._rightChild;
            }

            if (root._value.CompareTo(child._value) == -1) return;
            (child._value, root._value) = (root._value, child._value);
            Heapify(child);
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
            int current = 0;
            LinkedHeap<T> heap = this;
            do
            {
                //if (heap._value == null) yield break;
                yield return heap._value;

                current++;
                if (heap._leftChild == null)
                    yield break;
                

                //if (heap._value == null) yield break;
                //yield return heap._value;
                current++;


            } while (current < Count);
            
            
            if (_value == null) yield break;
            yield return _value;

            if (_leftChild != null)
            {
                foreach (var v in _leftChild)
                {
                    if (v == null) yield break;
                    yield return v;
                }
            }

            if (_rightChild != null)
            {
                foreach (var v in _rightChild)
                {
                    if (v == null) yield break;
                    yield return v;
                }
            } 
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
