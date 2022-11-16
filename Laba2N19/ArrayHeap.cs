using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2N19
{
    public class ArrayHeap<T> : IHeap<T> where T : IComparable<T>
    {
        private int _size;

        private T[] _arrayHeap;

        //private int StartPosition, EndPosition;

        public int Count { get; private set; }
        private bool _isEmpty;
        public bool IsEmpty { get { return Count == 0; } private set { _isEmpty = value; } }

        private bool IsFull()
        {
            return Count == _size;
        }

        public IEnumerable<T> Nodes { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            if (IsEmpty)
            {
                yield break;
            }
            int current = 0;
            do
            {
                yield return _arrayHeap[current];
                current = (current + 1);
            } while (current < Count);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private int NewSize()
        {
            //_size *= 2;
            return _size *= 2;
        }

        private int ParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        private int LeftChildIndex(int index)
        {
            return (index * 2) + 1;
        }

        private int RightChildIndex(int index)
        {
            return (index * 2) + 2;
        }

        private void Swap(int oldIndex, int newIndex)
        {
            T tmp = _arrayHeap[oldIndex];
            _arrayHeap[oldIndex] = _arrayHeap[newIndex];
            _arrayHeap[newIndex] = tmp;
        }
        public ArrayHeap()
        {
            _size = 4;
            Count = 0;
            _isEmpty = true;
            _arrayHeap = new T[_size];
        }

        public ArrayHeap(int size)
        {
            _size = size > 0 && size <= int.MaxValue ? size : 4;
            Count = 0;
            _isEmpty = true;
            _arrayHeap = new T[_size];
        }

        //public IEnumerator IEnumerable.GetEnumerator
        public void Add(T node)
        {
            if (IsFull())
            {
                _size *= 2;
                Array.Resize(ref _arrayHeap, _size);
            }

            int currentIndex = Count;
            _arrayHeap[currentIndex] = node;
            Count++;
            
            while (currentIndex > 0 && 
                _arrayHeap[currentIndex].CompareTo(_arrayHeap[ParentIndex(currentIndex)]) == -1)
            {
                Swap(currentIndex, ParentIndex(currentIndex));
                currentIndex = ParentIndex(currentIndex);
            }
        }
        public void Clear()
        {
            Array.Resize(ref _arrayHeap, 4);
            Count = 0;
        }
        public bool Contains(T node)
        {
            if (IsEmpty) return false;
            return Array.IndexOf(_arrayHeap, node) != -1; 
        }
        private void Heapify(int index)
        {
            while (index < Count)
            {
                int leftChildIndex = LeftChildIndex(index);
                if (leftChildIndex >= Count) break;

                int childIndex = leftChildIndex;
                int rightChildIndex = RightChildIndex(index);
                if (rightChildIndex < Count && _arrayHeap[rightChildIndex].CompareTo(_arrayHeap[leftChildIndex]) == -1)
                {
                    childIndex = rightChildIndex;
                }
                if (_arrayHeap[index].CompareTo(_arrayHeap[childIndex]) == -1) break;
                Swap(index, childIndex);
                index = childIndex;
            }
        }
        public void Remove(T node)
        {
            if (IsEmpty)
            {
                throw new HeapException("Cannot remove element from empty Min-Heap");
            }
            int index = Array.IndexOf(_arrayHeap, node);
            if (index == -1)
            {
                //throw new HeapException("Not found element in Min-Heap");
                return;
            }
            Count--;
            if (index < Count)
            {
                Array.Copy(_arrayHeap, index + 1, _arrayHeap, index, Count - index);
            }           
            Heapify(index);
            //return ret;
        }

        public void Print()
        {
            for (int i = 0; i < (Count / 2); i++)
            {
                Console.WriteLine("Parent : " + _arrayHeap[i]);
                if (LeftChildIndex(i) < Count)
                    Console.WriteLine(" Left : " + _arrayHeap[LeftChildIndex(i)]);
                if (RightChildIndex(i) < Count)
                    Console.WriteLine(" Right :" + _arrayHeap[RightChildIndex(i)]);
                Console.WriteLine();
            }
        }

    }
}
