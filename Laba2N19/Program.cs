using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 *   Задача направлена на создание обобщенных классов, описывающих
 * различные структуры данных, с применением итераторов, делегатов
 * и исключений. Класс должен содержать методы для отображения его значений.
 * Для каждой задачи также необходимо создать диаграмму классов,
 * поясняющую архитектуру проекта. Использовать встроенную функциональность
 * коллекций в .NET FCL запрещено.
 * 
 * Задание 19. Min-куча.
 * 
 *   Разработать обобщенный класс Heap<T> – класс для описания min-кучи.
 *   IHeap <T>: IEnumerable<T> – базовый интерфейс для всех min-куч;
 *   o   методы:
 *      void Add (T node);
 *      void Clear();
 *      bool Contains (T node);
 *      void Remove(T node);
 *   o   свойства:
 *      int Count;
 *      bool isEmpty;
 *      IEnumerable<T> nodes;
 *   HeapException – класс, описывающий исключения, которые могут
 * происходить в ходе работы с min-кучей (также можно написать ряд
 * наследников от HeapException);
 *   ArrayHeap < T >: IHeap < T > – класс min-кучи на основе массива;
 *   LinkedHeap < T >: IHeap < T > – класс min-кучи на основе связного
 * списка;
 *   UnmutableHeap < T >: IHeap< T > – класс неизменяющейся min-кучи,
 * является оберткой над любым существующей кучей(должен кидаться 
 * исключениями на вызов любого метода, изменяющего кучу);
 *   HeapUtils – класс различных операций над min-кучей;
 *   o   методы:
 *      static bool Exists< T >(IHeap < T >, CheckDelegate< T >);
 *      static IHeap < T > FindAll< T >(IHeap < T >, CheckDelegate<T>,
 *       HeapConstructorDelegate< T >);
 *      static void ForEach(IHeap < T >, ActionDelegate< T >);
 *      static bool CheckForAll< T >(IHeap < T >, CheckDelegate<T>);
 *   o   свойства:
 *      static readonly HeapConstructorDelegate< T >
 *       ArrayHeapConstructor;
 *      static readonly HeapConstructorDelegate< T >
 *       LinkedHeapConstructor;
 *   Также необходимо разработать серию примеров, демонстрирующих
 * основные аспекты работы с данной библиотекой min-куч.
 */

namespace Laba2N19
{
    internal class Program
    {       
        static void Main(string[] args)
        {
            ArrayHeap<int> arrayHeap = new ArrayHeap<int>();
            LinkedHeap<int> linkedHeap = new LinkedHeap<int>();

            Console.WriteLine("Testung ArrayHeap");
            TestHeap(arrayHeap);

            Console.WriteLine("Testung LinkedHeap");
            TestHeap(linkedHeap);

            Console.WriteLine("Testing universal utils with ArrayQueue");
            TestUniversalHeapUtils(arrayHeap);

            Console.WriteLine("Testing universal utils with LinkedQueue");
            TestUniversalHeapUtils(linkedHeap);

            Console.WriteLine("Testing ArrayQueue utils");
            TestArrayHeapUtils(arrayHeap);

            Console.WriteLine("Testing LinkedQueue utils");
            TestLinkedHeapUtils(linkedHeap);

            Console.ReadLine();
        }

        private static void TestUniversalHeapUtils(IHeap<int> heap)
        {
            heap.Add(-1);
            heap.Add(2);
            heap.Add(13);
            heap.Add(3);
            heap.Add(7);
            heap.Add(16);
            heap.Add(21);
            heap.Add(12);
            heap.Add(9);
            heap.Add(17);

            Console.WriteLine("Exists elements < 0?");
            Console.WriteLine(HeapUtils<int>.Exists(heap, elem => elem < 0));

            heap.Print();
            Console.WriteLine("After multiplying by 2");
            HeapUtils<int>.ForEach(heap, elem => elem * 2);
            heap.Print();

            Console.WriteLine("All elements > 0");
            Console.WriteLine(HeapUtils<int>.CheckForAll(heap, elem => elem > 0));

            Console.WriteLine();
        }

        private static void TestArrayHeapUtils(IHeap<int> heap)
        {
            IHeap<int> result = HeapUtils<int>.FindAll(heap, elem => elem > 0, HeapUtils<int>.ArrayHeapConstructer);
            result.Print();
            Console.WriteLine();
        }

        private static void TestLinkedHeapUtils(IHeap<int> heap)
        {
            IHeap<int> result = HeapUtils<int>.FindAll(heap, elem => elem > 0, HeapUtils<int>.LinkedHeapConstructer);
            result.Print();
            Console.WriteLine();
        }

        private static void TestHeap(IHeap<int> heap)
        {
            heap.Add(13);
            heap.Add(3);
            heap.Add(7);
            heap.Add(16);
            heap.Add(21);
            heap.Add(12);
            heap.Add(9);
            heap.Add(17);
            heap.Print();

            Console.WriteLine("After Removing 13");
            heap.Remove(13);
            heap.Print();

            Console.WriteLine("Clear Min-Heap");
            heap.Clear();
            heap.Print();

            Console.WriteLine();
        }
    }
}
