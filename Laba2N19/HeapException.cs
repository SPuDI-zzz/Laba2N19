using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2N19
{
    public class HeapException : Exception
    {
        public HeapException(string message) : base(message) { }
    }

    public class HeapOutOfBoundException : Exception
    {
        public object Value { get; }

        public HeapOutOfBoundException(string message, object value) : base(message)
        {
            Value = value;
        }
    }
}
