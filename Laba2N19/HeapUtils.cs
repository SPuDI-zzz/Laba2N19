using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2N19
{
    public class HeapUtils<T> where T : IComparable<T>
    {
        public delegate bool CheckDelegate<U>(T value);

        public delegate IHeap<T> HeapConstructorDelegate<K>();

        public delegate T ActionDelegate<U>(T value);

        public static readonly HeapConstructorDelegate<T> ArrayHeapConstructer = () => new ArrayHeap<T>(); 

        public static HeapConstructorDelegate<T> LinkedHeapConstructer = () => new LinkedHeap<T>();

        public static bool Exists(IHeap<T> heap, CheckDelegate<T> checkDelegate)
        {
            foreach (var item in heap)
            {
                if (checkDelegate(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static IHeap<T> FindAll(IHeap<T> tree, CheckDelegate<T> checkDelegate, HeapConstructorDelegate<T> constructorDelegate)
        {
            var tempHeap = constructorDelegate();

            foreach (var item in tree)
            {
                if (checkDelegate(item))
                {
                    tempHeap.Add(item);
                }
            }

            return tempHeap;
        }

        public static void ForEach(IHeap<T> heap, ActionDelegate<T> del)
        {
            ArrayHeap<T> tempArrayHeap = new ArrayHeap<T>();  
            IHeap<T> tempHeap = heap as ArrayHeap<T>;

            if (tempHeap != null)
            {
                foreach (var item in tempHeap)
                {
                    tempArrayHeap.Add(del(item));
                }

                heap.Clear();

                foreach (var item in tempArrayHeap)
                {
                    heap.Add(item);
                }

                return;
            }

            LinkedHeap<T> tempLinkedHeap = new LinkedHeap<T>();
            tempHeap = heap as LinkedHeap<T>;

            foreach (var item in tempHeap)
            {
                tempLinkedHeap.Add(del(item));
            }

            heap.Clear();

            foreach (var item in tempLinkedHeap)
            {
                heap.Add(item);
            }
        }

        public static bool CheckForAll(IHeap<T> tree, CheckDelegate<T> checkDelegate)
        {
            foreach (var item in tree)
            {
                if (!checkDelegate(item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
