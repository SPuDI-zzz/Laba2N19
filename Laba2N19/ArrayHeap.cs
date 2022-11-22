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

        public int Count { get; private set; }

        public bool IsEmpty { get { return Count == 0; } private set { } }

        public IEnumerable<T> Nodes { get; private set; }

        public ArrayHeap()
        {
            _size = 4;
            Count = 0;
            _arrayHeap = new T[_size];
        }

        public ArrayHeap(int size)
        {
            _size = size > 0 && size <= int.MaxValue ? size : 4;
            Count = 0;
            _arrayHeap = new T[_size];
        }

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

            _arrayHeap[index] = _arrayHeap[Count - 1];
            Count--;

            Heapify(index);
        }

        public void Clear()
        {
            _size = 4;
            Array.Resize(ref _arrayHeap, _size);
            Count = 0;
        }

        public bool Contains(T node)
        {
            if (IsEmpty) return false;
            return Array.IndexOf(_arrayHeap, node) != -1; 
        }

        public void Print()
        {
            for (int i = 0; i < (Count / 2); i++)
            {
                Console.WriteLine("Parent : " + _arrayHeap[i].ToString());
                if (LeftChildIndex(i) < Count)
                    Console.WriteLine(" Left : " + _arrayHeap[LeftChildIndex(i)].ToString());
                if (RightChildIndex(i) < Count)
                    Console.WriteLine(" Right : " + _arrayHeap[RightChildIndex(i)].ToString());
                Console.WriteLine();
            }
        }

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
                current++;
            } while (current < Count);
        }

        private bool IsFull()
        {
            return Count == _size;
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
            (_arrayHeap[newIndex], _arrayHeap[oldIndex]) = (_arrayHeap[oldIndex], _arrayHeap[newIndex]);
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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
