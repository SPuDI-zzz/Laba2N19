using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2N19
{
    public class HeapUtils<T> where T : IComparable<T>
    {
        public delegate bool CheckDelegate<T>(T value);

        public delegate IHeap<T> HeapConstructorDelegate<T>()/* where T : IComparable<T>*/;

        public delegate T ActionDelegate<T>(T value);

        public delegate TO ConvertDelegate<in TI, out TO>(TI value);

        public static readonly HeapConstructorDelegate<T> ArrayHeapConstructer = () => { return new ArrayHeap<T>(); };

        public static HeapConstructorDelegate<T> LinkedHeapConstructer = () => { return new LinkedHeap<T>(); };

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

        public static void ForEach(IHeap<T> tree, ActionDelegate<T> del)
        {
            var tempTree = tree as ArrayHeap<T>;
            var tempTree2 = tree as LinkedHeap<T>;
            if (tempTree != null)
            {
                foreach (var item in tempTree)
                {
                    tree.Remove(item);
                    tree.Add(del(item));
                }
                return;
            }
            foreach (var item in tempTree2)
            {
                tree.Remove(item);
                tree.Add(del(item));
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
