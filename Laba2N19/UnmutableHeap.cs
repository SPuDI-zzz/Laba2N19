using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2N19
{
    public class UnmutableHeap<T> : IHeap<T>
    {
        private readonly IHeap<T> _heap;

        public int Count { get; }

        public bool IsEmpty { get { return Count == 0; } }

        public IEnumerable<T> Nodes { get; }

        public UnmutableHeap(IHeap<T> heap)
        {
            _heap = heap;
            Count = heap.Count;
        }

        public void Clear()
        {
            throw new HeapException("Trying to clear an immutable Min-Heap");
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _heap.GetEnumerator();
        }

        public void Add(T node)
        {
            throw new HeapException("Trying to add an immutable Min-Heap");
        }

        public void Remove(T node)
        {
            throw new HeapException("Trying to remove an immutable Min-Heap");
        }

        public bool Contains(T node)
        {
            return _heap.Contains(node);
        }

        public void Print()
        {
            _heap.Print();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
