using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2N19
{
    public interface IHeap<T> : IEnumerable<T>
    {
        int Count { get; }
        bool IsEmpty { get; }
        IEnumerable<T> Nodes { get; }
        void Add(T node);
        void Clear();
        bool Contains(T node);
        void Remove(T node);
        void Print();
    }
}
